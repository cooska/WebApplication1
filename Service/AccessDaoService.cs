using cardapi.Models.SqlData;
using cardapi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Service {
    public class AccessDaoService : IAccessDao {
        public DbHelperAccess mySQL;
        public AccessDaoService(DbHelperAccess context) {
            mySQL = context;
        }
        public useractive GetAcitedInfo(string schoolnumber) {
            return mySQL.User_Active.FirstOrDefault(o => o.u_school_num == schoolnumber);
        }

        public bool InsertActivedInfo(string username, string schoolnum, string phonenum) {
            mySQL.User_Active.Add(new useractive {
                u_school_num = schoolnum,
                u_name = username,
                u_phone = phonenum,
                u_active_time = DateTime.Now
            });
            return mySQL.SaveChanges() > 0;

        }
    }
}
