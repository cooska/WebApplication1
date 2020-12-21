using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapi.Models.Attributes;

namespace cardapi.Models.WeInfo {
    [Portal(ProtalName = "http://210.43.64.135:8082/updatePassword", HttpMethod = HttpMethodEnum.GET)]
    public class DakeUpdatePassReq : IReqBase {
        public string username { get; set; }
        public string newpassword { get; set; }
    }

    public class DakeUpdatePassResp : WeResponseBase {

    }
}
