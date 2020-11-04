using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class FormModel
    {
        public string username { get; set; }
        public string schoolnum { get; set; }
        public string department { get; set; }
        public string idcard { get; set; }
        public string password { get; set; }
        public string repassword { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string verify { get; set; }
        public DateTime verify_time { get; set; }
    }
}
