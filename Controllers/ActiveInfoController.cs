using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Service;
using System.Reflection;
using System.Web;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.WeInfo;

namespace WebApplication1.Controllers
{
    public class ActiveInfoController : Controller
    {
        private readonly ILogger<ActiveInfoController> _logger;

        readonly ISmsSend _sms;
        readonly IPortalHttpSend<IReqBase, IRespBase> _portalService;
        public ActiveInfoController(ISmsSend sms)
        {
            _sms = sms;
        }
        public IActionResult Index()
        {
            var cookiedata = Request.Cookies["loginuser"];
            if(string.IsNullOrEmpty(cookiedata)) {
                return Redirect("/Home/Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(FormModel f)
        {
            var cookie = new FormModel();
            if (Request.Cookies.ContainsKey(f.mobile)) {
                var cookiedata = Request.Cookies[f.mobile];
                cookie = JsonConvert.DeserializeObject<FormModel>(cookiedata);
                if (cookie != null && (cookie.verify_time.AddMinutes(5) < DateTime.Now || f.verify != cookie.verify || f.password!=f.repassword))
                    return Index();

                var user = Request.Cookies["loginuser"];
                var userdata= JsonConvert.DeserializeObject<FormModel>(user);
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
                if(departinfos == null)
                    return Content("用户没有机构信息");
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
                    return Content("用户没有机构信息");
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
                        return Content("激活成功");
                    else
                        return Content("激活失败,请联系管理员");
                }
            }

            //if (resp.retCode == "0")
            //    return Redirect("/Home/Index");
            return Redirect("/Home/Index");
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
        public void PostSms([FromBody] FormModel f)
        {
            var cookie = new FormModel();
            if(Request.Cookies.ContainsKey(f.mobile)) {
                var cookiedata = Request.Cookies[f.mobile];
                cookie = JsonConvert.DeserializeObject<FormModel>(cookiedata);
                if (cookie != null && cookie.verify_time.AddMinutes(1) > DateTime.Now)
                    return;
            }
            if (Request.Cookies.ContainsKey(f.mobile)) Response.Cookies.Delete(f.mobile);
            var code = _sms.SendSms(f.mobile);
            if (string.IsNullOrEmpty(code))
                return;
            f.verify = code;
            f.verify_time = DateTime.Now;
            CookieOptions options = new CookieOptions();
            options.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            Response.Cookies.Append(f.mobile, JsonConvert.SerializeObject(f),options);
            //return Redirect("/Home/Index");
        }


    }
}
