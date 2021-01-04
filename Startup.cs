using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Jet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using cardapi.Models;
using cardapi.Service;
using cardapi.Tools;

namespace cardapi {
    public class Startup {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,

                builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()

                );

            });
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddSingleton<ISmsSend, SmsSendServer>();
            services.AddSingleton<IPortalHttpSend<IReqBase,IRespBase>, PortalHttpSendService<IReqBase, IRespBase>>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>

            {

                //��¼·�������ǵ��û���ͼ������Դ��δ���������֤ʱ�����򽫻Ὣ�����ض���������·��

                o.LoginPath = new PathString("/Home/Index");

                //��ֹ����·�������û���ͼ������Դʱ����δͨ������Դ���κ���Ȩ���ԣ����󽫱��ض���������·����

                o.AccessDeniedPath = new PathString("/Home/Privacy");

            });
            services.AddScoped<IDao, DaoService>();
            services.AddScoped<IAccessDao, AccessDaoService>();
            var conn = Configuration.GetConnectionString("MysqlConnection");
            var accessconn =  string.Format(Configuration.GetConnectionString("AccessConnection"), AppContext.BaseDirectory);
            services.AddDbContext<DbHelperMySQL>(option => option.UseMySql(conn));
            services.AddDbContext<DbHelperAccess>(option => option.UseJet(accessconn));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
