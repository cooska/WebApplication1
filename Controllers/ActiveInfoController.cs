﻿using System;
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
            }

            //if (resp.retCode == "0")
            //    return Redirect("/Home/Index");
            return Redirect("/Home/Index");
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
