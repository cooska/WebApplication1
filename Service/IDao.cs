using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Service {
    public interface IDao {
        bool CheckLogin(string xm, string xgh, string sfz);
    }
}
