using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MvcProject.Entities;
using System.Reflection;

namespace MvcProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            /* Razor Runtime paketini de çalýþtýrmasý için */
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
              //  opts.UseLazyLoadingProxies();//Ýliþkisel tablolar için
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(opts =>
                {
                    opts.Cookie.Name = "myCookie.auth";
                    opts.ExpireTimeSpan = TimeSpan.FromDays(7);
                    opts.SlidingExpiration = false;
                    opts.LoginPath = "/Account/Login";
                    opts.LogoutPath = "/Account/Logout";
                    opts.AccessDeniedPath = "/Home/AccessDeniced";

                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            //Cokkieden okuyup girmesi için
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}