﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "cgi-bin/user/create?access_token={access_token}", HttpMethod = HttpMethodEnum.POSTLINE)]
    public class AddUserReq : UserInfo {
        public string access_token { get; set; }
    }

    public class AddUserResp : WeInfoModelBase {
    }
}
