using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.WeInfo {
    public class WeResponseBase:IRespBase {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
}
