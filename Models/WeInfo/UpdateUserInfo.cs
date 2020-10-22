using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "cgi-bin/user/update?access_token={access_token}", HttpMethod = HttpMethodEnum.POSTLINE)]
    public class UpdateUserInfoReq:UserInfo {
        public string access_token { get; set; }
    }

    public class UpdateUserInfoResp : WeResponseBase {

    }
}
