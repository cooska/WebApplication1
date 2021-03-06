﻿using cardapi.Models.SqlData;
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

    public interface IAccessDao {
        useractive GetAcitedInfo(string schoolnumber);
        bool InsertActivedInfo(string username, string schoolnum, string phonenum);
    }
}
