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

namespace WebApplication1.Controllers
{
    public class ActiveInfoController : Controller
    {
        private readonly ILogger<ActiveInfoController> _logger;

        readonly ISmsSend _sms;
        public ActiveInfoController(ISmsSend sms)
        {
            _sms = sms;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save()
        {
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public async Task< IActionResult > PostSms([FromBody] FormModel f)
        {
            _sms.SendSms(f.mobile);
            return Redirect("/Home/Index");
        }


    }
}
