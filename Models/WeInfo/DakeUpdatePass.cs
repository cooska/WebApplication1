using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "http://ims.jsu.edu.cn/ims/api/account/update/{userid}/userpassword", HttpMethod = HttpMethodEnum.POSTLINE)]
    public class DakeUpdatePassReq : IReqBase {
        public string userid { get; set; }
        public string value { get; set; }
    }

    public class DakeUpdatePassResp : WeResponseBase {

    }
}
