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

    [Portal(ProtalName = "http://210.43.64.135:8082/view", HttpMethod = HttpMethodEnum.GET)]
    public class DakeGetUserInfoReq : IReqBase {
        public string username { get; set; }
    }

    public class DakeGetUserInfoResp : WeResponseBase {
        public DakeUserAccount data { get; set; }
    }
    public class DakeUserAccount {
        public DakeUserInfo account { get; set; }
    }
    public class DakeUserInfo {
        public string mail { get; set; }
        public string eduOrgID { get; set; }
        public string eduOrgCn { get; set; }
        public string cn { get; set; }
        public string phone { get; set; }
        public string accountId { get; set; }
    }

}
