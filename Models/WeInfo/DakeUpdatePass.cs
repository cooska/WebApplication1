using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    public enum DakeEnum {
        userpassword,
        mail,
        telephonenumber
    }
    [Portal(ProtalName = "http://210.43.64.135:8082/update", HttpMethod = HttpMethodEnum.GET)]
    public class DakeUpdatePassReq : IReqBase {
        public string username { get; set; }
        public string newvalue { get; set; }
        public string method { get; set; }
    }

    public class DakeUpdatePassResp : WeResponseBase {

    }
}
