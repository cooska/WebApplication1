using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "api/org/list", HttpMethod = HttpMethodEnum.GET)]
    public class GetTagListReq : IReqBase {
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
