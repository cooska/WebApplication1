using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapi.Models.Attributes;

namespace cardapi.Models.WeInfo {
    [Portal(ProtalName = "api/org/list", HttpMethod = HttpMethodEnum.GET)]
    public class GetTagListReq : IReqBase {
        public string access_token { get; set; }
        public string devToken { get; set; }
    }

    public class GetTagListResp: WeInfoModelBase {
        public List<TagInfo> tagList { get; set; }
    }

    public class TagInfo {
        public string tagId { get; set; }
        public string tagName { get; set; }
    }
}
