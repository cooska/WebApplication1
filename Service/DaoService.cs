using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Tools;

namespace WebApplication1.Service {
    public class DaoService : IDao {
        public DbHelperMySQL mySQL;
        public DaoService(DbHelperMySQL context) {
            mySQL = context;
        }
        public bool CheckLogin(string xm, string xgh, string sfz) {
            object yhxx = null;
            if (xgh.Length <= 6 && xgh != "test")
                yhxx = mySQL.T_Yhxxbs.SingleOrDefault(o => o.xm == xm && o.zgh == xgh && o.sfzjh == sfz);
            else
                yhxx = mySQL.S_Yhxxbs.SingleOrDefault(o => o.xm == xm && o.xh == xgh && o.sfzjh == sfz);
            return yhxx != null;
        }

        public string GetDepartment(string xgh) {
            if (xgh.Length <= 6 && xgh != "test") {
                var yhxx = mySQL.T_Yhxxbs.SingleOrDefault(o => o.zgh == xgh);
                if (yhxx != null) {
                    var dwxx = mySQL.Dwxxb.SingleOrDefault(o => o.dwdm == yhxx.szdw);
                    if(dwxx!=null)
                        return dwxx.dwmc;
                }
            } else {
                var yhxx = mySQL.S_Yhxxbs.SingleOrDefault(o => o.xh == xgh);
                if (yhxx != null) {
                    var dwxx = mySQL.Bjxxb.SingleOrDefault(o => o.bjdm == yhxx.bjdm);
                    if (dwxx != null)
                        return dwxx.bjmc;
                }

            }
            return "";
        }
    }
}
