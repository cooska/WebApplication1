using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.SqlData;

namespace WebApplication1.Service {
    public interface IDao {
        bool CheckLogin(string xm, string xgh, string sfz);
        string GetDepartment(string xgh);

        List<CheckNullMobileModel> GetCheckNullMobiles();
    }

    public interface IAccessDao {
        useractive GetAcitedInfo(string schoolnumber);
        bool InsertActivedInfo(string username, string schoolnum, string phonenum);
    }
}
