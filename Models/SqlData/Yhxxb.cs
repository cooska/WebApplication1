using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.SqlData {
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
    }

    [Table("t_jcsj_dw")]
    public class dwxxb {
        [Key]
        public string dwdm { get; set; }
        public string dwmc { get; set; }
    }

}
