using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models.SqlData {
    [Table("t_jcsj_jsjbxx")]
    public class Yhxxb {
        [Key]
        public string pID { get; set; }
        public string zgh { get; set; }
        public string xm { get; set; }
        public string sfzjh { get; set; }
        public string szdw { get; set; }
        public string yddh { get; set; }
    }

    [Table("t_jcsj_xsjbxx")]
    public class xsxxb {
        [Key]
        public string pID { get; set; }
        public string xh { get; set; }
        public string xm { get; set; }
        public string sfzjh { get; set; }
        public string yddh { get; set; }
        public string yxdm { get; set; }
        public string bjdm { get; set; }
    }

    [Table("t_jcsj_dw")]
    public class dwxxb {
        [Key]
        public string dwdm { get; set; }
        public string dwmc { get; set; }
    }

    [Table("t_jcsj_bj")]
    public class bjxxb {
        [Key]
        public string bjdm { get; set; }
        public string bjmc { get; set; }
    }

    [Table("u_active")]
    public class useractive {
        [Key]
        public int id { get; set; }
        public string u_name { get; set; }
        public string u_school_num { get; set; }
        public string u_phone { get; set; }
        public DateTime u_active_time { get; set; }
    }
}
