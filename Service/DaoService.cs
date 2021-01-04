using cardapi.Models.SqlData;
using cardapi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Service {
    public class DaoService : IDao {
        public DbHelperMySQL mySQL;
        public DaoService(DbHelperMySQL context) {
            mySQL = context;
        }
        public bool CheckLogin(string xm, string xgh, string sfz) {
            object yhxx = null;
            try {
                if (xgh.Length <= 6 && xgh != "test")
                    yhxx = mySQL.T_Yhxxbs.SingleOrDefault(o => o.xm == xm && o.zgh == xgh && o.sfzjh == (string.IsNullOrEmpty(sfz)?null:sfz));
                else
                    yhxx = mySQL.S_Yhxxbs.SingleOrDefault(o => o.xm == xm && o.xh == xgh && o.sfzjh == (string.IsNullOrEmpty(sfz) ? null : sfz));
            } catch (Exception ex) {
            }
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

        public string GetMobile(string userid) {
            if (string.IsNullOrEmpty(userid))
                return "";
            object yhxx = null;
            try {
                if (userid.Length <= 6 && userid != "test") {
                    yhxx = mySQL.T_Yhxxbs.SingleOrDefault(o => o.zgh == userid);
                    if (yhxx != null)
                        return ((Yhxxb)yhxx).yddh;
                } else {
                    yhxx = mySQL.S_Yhxxbs.SingleOrDefault(o => o.xh == userid);
                    if (yhxx != null)
                        return ((xsxxb)yhxx).yddh;
                }
            } catch (Exception ex) {
                return "";
            }
            return "";
        }

    }
}
