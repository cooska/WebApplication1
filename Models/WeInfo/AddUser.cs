using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Models.WeInfo {
    [Portal(ProtalName = "api/org/addTagUsers?devToken={devToken}", HttpMethod = HttpMethodEnum.POSTLINE)]
    public class AddUserReq : IReqBase {
        public string devToken { get; set; }
        public string tagId { get; set; }
        public List<string> userIdList { get; set; }
        public List<string> partyIdList { get; set; }
    }

    public class AddUserResp : WeInfoModelBase {
        public string invalUserIds { get; set; }
        public List<string> invalPartyIds { get; set; }
    }
}
