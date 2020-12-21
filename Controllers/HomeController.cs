using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BakClass.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cardapi.Models;
using cardapi.Models.SqlData;
using cardapi.Models.WeInfo;
using cardapi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace cardapi.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ISmsSend _sms;
        private readonly IDao dao;
        public HomeController(ILogger<HomeController> logger,IDao dbContext, ISmsSend sms)
        {
            _logger = logger;
            dao = dbContext;
            _sms = sms;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Login([FromBody] FormModel logindata) {
            if (dao.CheckLogin(logindata.username, logindata.schoolnum, logindata.idcard)) {
                var jgdm = dao.GetDepartment(logindata.schoolnum);
                if (Request.Cookies.ContainsKey(logindata.schoolnum))
                    Response.Cookies.Delete(logindata.schoolnum);
                logindata.department = jgdm;
                Response.Cookies.Append(logindata.schoolnum, JsonConvert.SerializeObject(logindata), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });

                var ret = JsonConvert.SerializeObject(new WeResponseBase {
                    errcode = 0,
                    result = "/ActiveInfo/Index"
                });
                return Content(ret);
            } else
                return Content(WeInfoService.ShowErr("登录失败"));
        }
        [HttpPost]
        public IActionResult Save([FromBody] FormModel f) {
            var cookie = new FormModel();
            if (Request.Cookies.ContainsKey(f.schoolnum)) {
                var cookiedata = Request.Cookies[f.schoolnum];
                cookie = JsonConvert.DeserializeObject<FormModel>(cookiedata);
                if (cookie != null && (cookie.verify_time.AddMinutes(5) < DateTime.Now || f.verify != cookie.verify || f.password != f.repassword))
                    return Content(WeInfoService.ShowErr("激活失败,请联系管理员"));

                var user = Request.Cookies[f.schoolnum];
                var userdata = JsonConvert.DeserializeObject<FormModel>(user);
                f.username = userdata.username;
                f.idcard = userdata.idcard;
                f.schoolnum = userdata.schoolnum;
                f.department = userdata.department;

                var tokendata = WeInfoService.GetToken();
                var department = WeInfoService.GetDepartment(tokendata.access_token);
                var usertype = 17;//学生
                if (f.schoolnum.Length <= 6 && f.schoolnum != "test")
                    usertype = 15; //教师
                var departinfos = department.Where(o => o.name == f.department).ToList();
                if (departinfos == null)
                    return Content(WeInfoService.ShowErr("用户没有机构信息"));
                var departid = 0;
                if (departinfos.Count == 1)
                    departid = departinfos[0].id;
                else {
                    foreach (var item in departinfos) {
                        departid = GetTypeId(department, item.id, usertype);
                        if (departid > 0) {
                            departid = item.id;
                            break;
                        }
                    }
                }
                if (departid == 0)
                    return Content(WeInfoService.ShowErr("用户没有机构信息"));
                if (tokendata != null && tokendata.errcode == 0) {
                    var weuserdata = WeInfoService.GetUserInfo(tokendata.access_token, f.schoolnum);
                    var b = false;
                    if (weuserdata == null || weuserdata.errcode == 60111) {
                        b = WeInfoService.CreateUserInfo(new AddUserReq {
                            access_token = tokendata.access_token,
                            name = f.username,
                            userid = f.schoolnum,
                            mobile = f.mobile,
                            email = f.email,
                            department = new List<int> { departid }
                        });
                    } else {
                        b = WeInfoService.UpdateUserInfo(new UpdateUserInfoReq {
                            access_token = tokendata.access_token,
                            department = new List<int> { departid },
                            userid = f.schoolnum,
                            name = f.username,
                            mobile = f.mobile,
                            email = f.email
                        });
                    }
                    if (b) {
                        b = WeInfoService.UpdateDakePassword(f.schoolnum, f.password);
                    }
                    if (b)
                        return Content(JsonConvert.SerializeObject(new WeResponseBase {
                            errcode = 0,
                            result = "激活成功"
                        }));
                    else
                        return Content(WeInfoService.ShowErr("激活失败,请联系管理员"));
                }
            }

            //if (resp.retCode == "0")
            //    return Redirect("/Home/Index");
            return Content(WeInfoService.ShowErr("激活失败,请联系管理员"));
        }

        private int GetTypeId(List<DepartmentItem> deps, int did, int pid) {
            var dep = deps.SingleOrDefault(o => o.id == did);
            if (dep == null)
                return 0;
            if (dep.parentid == pid)
                return dep.id;
            return GetTypeId(deps, dep.parentid, pid);
        }

        [HttpPost]
        public IActionResult PostSms([FromBody] FormModel f) {
            var cookie = new FormModel();
            if (Request.Cookies.ContainsKey(f.schoolnum)) {
                var cookiedata = Request.Cookies[f.schoolnum];
                cookie = JsonConvert.DeserializeObject<FormModel>(cookiedata);
                if (cookie != null && cookie.verify_time.AddMinutes(1) > DateTime.Now)
                    return Content(JsonConvert.SerializeObject(new WeResponseBase {
                        errcode = 1,
                        result = "请一分钟后再获取验证码"
                    }));
                ;
            }
            if (Request.Cookies.ContainsKey(f.schoolnum))
                Response.Cookies.Delete(f.schoolnum);
            var code = _sms.SendSms(f.mobile);
            if (string.IsNullOrEmpty(code))
                return Content(JsonConvert.SerializeObject(new WeResponseBase {
                    errcode = 2,
                    result = "验证码发送失败"
                }));
            ;
            f.verify = code;
            f.verify_time = DateTime.Now;
            CookieOptions options = new CookieOptions();
            options.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            Response.Cookies.Append(f.schoolnum, JsonConvert.SerializeObject(f), options);
            return Content(JsonConvert.SerializeObject(new WeResponseBase {
                errcode = 0,
                result = "验证码发送成功"
            }));
        }

        [HttpGet]
        [Route("~/api/Home/GetMobile/{userid}")]
        public string GetMobile(string userid) {
            var res = dao.GetMobile(userid);
            return res;
        }
        //[HttpPost]
        //public ActionResult Login(string username, string schoolnum, string idcard)

        //{
        //    //string url = "http://127.0.0.1/test"; //接口请求地址
        //    //string boundary = DateTime.Now.Ticks.ToString("X");
        //    //var formData = new MultipartFormDataContent(boundary);
        //    //formData.Add(new StringContent(Request.Form["num1"]), "parametername");//参数一
        //    //formData.Add(new StringContent(Request.Form["num2"]), "parametername");//参数二
        //    //HttpClient httpClient = new HttpClient();
        //    //var response = httpClient.PostAsync(url, formData).Result;
        //    //var responseContent = response.Content.ReadAsStringAsync().Result;//获取接口返回的值
        //    //var json= Json(responseContent);

        //    //if (username.Equals("admin") && schoolnum.Equals("123456"))

        //    //{



        //    //}
        //    if (dao.CheckLogin(username, schoolnum, idcard)) {
        //        var jgdm = dao.GetDepartment(schoolnum);
        //        if (Request.Cookies.ContainsKey("loginuser"))
        //            Response.Cookies.Delete("loginuser");
        //        var user = new FormModel {
        //            username = username,
        //            schoolnum = schoolnum,
        //            idcard = idcard,
        //            department = jgdm
        //        };
        //        Response.Cookies.Append("loginuser", JsonConvert.SerializeObject(user), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
        //        return Redirect("/ActiveInfo/Index"); 
        //    }
        //    else
        //        return Content("登录失败");
        //}


    }
}
