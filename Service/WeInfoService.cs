using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.WeInfo;

namespace WebApplication1.Service {
    public class WeInfoService {
        /// <summary>
        /// 获取token授权
        /// </summary>
        /// <returns></returns>
        public GetDevTokenResp GetToken() {
            GetDevTokenResp resp;
            var serve = new PortalHttpSendService<GetDevTokenReq, GetDevTokenResp>();
            serve.GetJsonData(new GetDevTokenReq {
                devId = "",
                devSecret = ""
            }, out resp);
            return resp;
        }

        public bool AddUser()
    }
}
