using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal (ProtalName = "api/token/getDevToken",HttpMethod = HttpMethodEnum.GET)]
    public class GetDevTokenReq:IReqBase {
        public string devId { get; set; }
        public string devSecret { get; set; }
    }

    public class GetDevTokenResp: WeInfoModelBase {
        public string devToken { get; set; }
    }

}
