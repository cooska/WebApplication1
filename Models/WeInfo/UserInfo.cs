using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models.WeInfo {
    public class Text {
        /// <summary>
        /// 文本
        /// </summary>
        public string value { get; set; }
    }

    public class AttrsItem {
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文本名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Text text { get; set; }
    }

    public class Extattr {
        /// <summary>
        /// 
        /// </summary>
        public List<AttrsItem> attrs { get; set; }
    }

    public class External_attrItem {
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文本名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Text text { get; set; }
    }

    public class External_profile {
        /// <summary>
        /// 企业简称
        /// </summary>
        public string external_corp_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<External_attrItem> external_attr { get; set; }
    }

    public class UserInfo: WeResponseBase,IReqBase {
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 李四
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> department { get; set; }
        public int enable { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public List<int> order { get; set; }
        /// <summary>
        /// 后台工程师
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> is_leader_in_dept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string thumb_avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string alias { get; set; }
        /// <summary>
        /// 广州市海珠区新港中路
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string open_userid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int main_department { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Extattr extattr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qr_code { get; set; }
        /// <summary>
        /// 产品经理
        /// </summary>
        public string external_position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public External_profile external_profile { get; set; }
    }
}
