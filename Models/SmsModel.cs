using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApplication1.Models
{
    public class SmsModel
    {
        public string mobile { get; set; }
    }
    [XmlType("WsSubmitTempletReq")]
    public class CMSmsRequest {
        [XmlElement("apId")]
        public string apId { get; set; }
        [XmlElement("secretKey")]
        public string secretKey { get; set; }
        [XmlElement("mac")]
        public string mac { get; set; }
        [XmlElement("addSerial")]
        public string addSerial { get; set; }
        [XmlElement("sign")]
        public string sign { get; set; }
        [XmlArray("params")]
        public string @params { get; set; }
        [XmlArray("mobiles")]
        public string mobiles { get; set; }
        [XmlElement("ecName")]
        public string ecName { get; set; }
        [XmlElement("templateId")]
        public string templateId { get; set; }

    }

}
