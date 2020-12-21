using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models.WeInfo {
    public class WeInfoModelBase :IRespBase {
        public string retCode { get; set; }
        public string retMessage { get; set; }
        public string body { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
}
