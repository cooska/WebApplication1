﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models.Attributes {
    public class AttributeModel {
        public string PortalName { get; set; }
        public HttpMethodEnum MethodEnum { get; set; }
    }
}
