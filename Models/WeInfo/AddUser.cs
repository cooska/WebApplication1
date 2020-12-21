using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapi.Models.Attributes;

namespace cardapi.Models.WeInfo {
    [Portal(ProtalName = "cgi-bin/user/create?access_token={access_token}", HttpMethod = HttpMethodEnum.POSTLINE)]
    public class AddUserReq :IReqBase {
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 李四
        /// </summary>
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public List<int> department { get; set; }
    }

    public class AddUserResp : WeInfoModelBase {
    }
}
