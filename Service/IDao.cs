using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Service {
    public interface IDao {
        bool CheckLogin(string xm, string xgh, string sfz);
        string GetDepartment(string xgh);
        string GetMobile(string userid);
    }
}
