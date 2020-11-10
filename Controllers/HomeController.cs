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
using WebApplication1.Models;
using WebApplication1.Models.SqlData;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDao dao;
        public HomeController(ILogger<HomeController> logger,IDao dbContext)
        {
            _logger = logger;
            dao = dbContext;
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
            if (dao.CheckLogin(username, schoolnum, idcard)) {
                var jgdm = dao.GetDepartment(schoolnum);
                if (Request.Cookies.ContainsKey("loginuser"))
                    Response.Cookies.Delete("loginuser");
                var user = new FormModel {
                    username = username,
                    schoolnum = schoolnum,
                    idcard = idcard,
                    department = "101" + jgdm
                };
                Response.Cookies.Append("loginuser", JsonConvert.SerializeObject(user), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
                return Redirect("/ActiveInfo/Index"); 
            }
            else
                return Content("登录失败");
        }


    }
}
