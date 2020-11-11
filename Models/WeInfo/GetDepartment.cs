using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "cgi-bin/department/list", HttpMethod = HttpMethodEnum.GET)]
    public class GetDepartmentReq:IReqBase {
        public string access_token { get; set; }
        public int id { get; set; } = 1;
    }

    public class GetDepartmentResp : WeInfoModelBase {
        public List<DepartmentItem> department { get; set; }

    }
    public class DepartmentItem {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 吉首大学
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int parentid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
    }

}
