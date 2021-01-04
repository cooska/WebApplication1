using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.SqlData;

namespace WebApplication1.Tools {
    public class DbHelperAccess:DbContext {
        public DbHelperAccess(DbContextOptions<DbHelperAccess> options) : base(options) {

        }

        public DbSet<useractive> User_Active { get; set; }
    }
}
