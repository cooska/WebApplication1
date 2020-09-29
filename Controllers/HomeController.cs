using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public async Task<IActionResult> Login(string username, string schoolnum, string idcard)

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
            return Redirect("/ActiveInfo/Index");
        }

        
    }
}
