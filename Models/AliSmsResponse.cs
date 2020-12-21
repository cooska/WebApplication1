using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models {
    public class AliSmsResponse {
        public string Message { get; set; }
        public string RequestId { get; set; }
        public string Code { get; set; }
    }
}
