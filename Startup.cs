using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakClass.Tools;
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

                //登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径

                o.LoginPath = new PathString("/Home/Index");

                //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径。

                o.AccessDeniedPath = new PathString("/Home/Privacy");

            });
            services.AddScoped<IDao, DaoService>();
            var conn = Configuration.GetConnectionString("MysqlConnection");
            services.AddDbContext<DbHelperMySQL>(option => option.UseMySql(conn));
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
                    pattern: "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
