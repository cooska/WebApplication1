using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Models.SqlData;
using WebApplication1.Models.WeInfo;
using WebApplication1.Service;

namespace WebApplication1.Controllers {
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDao dao;
        private readonly IAccessDao acdao;
        public HomeController(ILogger<HomeController> logger,IDao dbContext, IAccessDao dbAccess)
        {
            _logger = logger;
            dao = dbContext;
            acdao = dbAccess;
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
        public ActionResult Login(string username, string schoolnum, string idcard)

        {
            //string url = "http://127.0.0.1/test"; //接口请求地址
            //string boundary = DateTime.Now.Ticks.ToString("X");
            //var formData = new MultipartFormDataContent(boundary);
            //formData.Add(new StringContent(Request.Form["num1"]), "parametername");//参数一
            //formData.Add(new StringContent(Request.Form["num2"]), "parametername");//参数二
            //HttpClient httpClient = new HttpClient();
            //var response = httpClient.PostAsync(url, formData).Result;
            //var responseContent = response.Content.ReadAsStringAsync().Result;//获取接口返回的值
            //var json= Json(responseContent);

            //if (username.Equals("admin") && schoolnum.Equals("123456"))

            //{



            //}
            //if (acdao.GetAcitedInfo(schoolnum) != null) {
            //    return Content("您的账户已经激活，请勿重复操作");
            //} else {
                if (dao.CheckLogin(username, schoolnum, idcard)) {
                    var jgdm = dao.GetDepartment(schoolnum);
                    if (Request.Cookies.ContainsKey("loginuser"))
                        Response.Cookies.Delete("loginuser");
                    var user = new FormModel {
                        username = username,
                        schoolnum = schoolnum,
                        idcard = idcard,
                        department = jgdm
                    };
                    Response.Cookies.Append("loginuser", JsonConvert.SerializeObject(user), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
                    return Redirect("/ActiveInfo/Index");
                } else
                    return Content("您提交的信息和系统预留信息不符，请确认输入正确。也有可能系统预留信息有误，请与管理员联系。");
            //}
        }


        public void GetNullMobile() {
            var list = dao.GetCheckNullMobiles();
            var templist = new List<List<CheckNullMobileModel>>();
            int n = list.Count % 200 + 1;
            for(int i = 0; i <= n; i++) {
                templist.Add(list.GetRange(i * 200, 200));
            }
            var tokendata = WeInfoService.GetToken();
            foreach(var ll in templist) {
                new Thread(() => {
                    foreach (var l in ll) {
                        var weuserdata = WeInfoService.GetUserInfo(tokendata.access_token, l.schoolnum);
                        if (weuserdata != null) {
                            l.mobile = weuserdata.mobile;
                        }
                    }
                    foreach (var l in ll) {
                        if (string.IsNullOrEmpty(l.mobile))
                            continue;
                        var b = WeInfoService.UpdateDakePassword(l.schoolnum, l.mobile, DakeEnum.telephonenumber);
                        if (!b)
                            Console.WriteLine(l.mobile + ":" + l.schoolnum);
                        Thread.Sleep(1000);
                    }
                }).Start();
            }
        }


    }
}
