using Identity.Context;
using Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt => {
                //Password Options
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                //SignIn Options
                //opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<IdentityContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.LogoutPath = new PathString("/Home/SignOut");            
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.Cookie.Name = "Identity1";
                opt.SlidingExpiration = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(20); 
            });
            services.AddDbContext<IdentityContext>(opt =>
            {
                opt.UseSqlServer("Data Source=DESKTOP-473BNVL; Initial Catalog=IdentityApp1; User id=sa; Password=mssql1234;");
            });
            services.AddControllersWithViews(); //MVC
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
