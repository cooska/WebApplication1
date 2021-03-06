﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapi.Models.Attributes;

namespace cardapi.Models.WeInfo {
    [Portal (ProtalName = "cgi-bin/gettoken", HttpMethod = HttpMethodEnum.GET)]
    public class GetDevTokenReq:IReqBase {
        public string corpid { get; set; }
        public string corpsecret { get; set; }
    }

    public class GetDevTokenResp: WeInfoModelBase {
        public string access_token { get; set; }
        public long expires_in { get;set; }
    }

}
