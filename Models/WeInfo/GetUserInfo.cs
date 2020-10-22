using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal (ProtalName = "cgi-bin/user/get", HttpMethod = HttpMethodEnum.GET)]
    public class GetUserInfoReq:IReqBase {
        public string access_token { get; set; }
        public string userid { get; set; }
    }

    public class GetUserInfoResp : UserInfo {

    }
}
