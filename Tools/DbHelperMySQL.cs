using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.SqlData;

namespace BakClass.Tools
{
    /// <summary>
    /// 数据访问抽象基础类
    /// Copyright (C) Maticsoft
    /// </summary>
    public class DbHelperMySQL:DbContext
    {
        public DbHelperMySQL(DbContextOptions<DbHelperMySQL> options) :base(options)
        {            
        }

        public DbSet<Yhxxb> T_Yhxxbs { get; set; }
        public DbSet<xsxxb> S_Yhxxbs { get; set; }
    }

}
