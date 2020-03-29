using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace TTFrontEnd
{
    public static class ExtendConfig
    {
        const string CONNECTION_STRING_NAME = "DbConnectString";
        const string API_BACK_END = "ApiBackEnd";
        public static IServiceCollection InsideConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddDbContext<SW_InsideContext>(options =>
            //options.UseNpgsql(Configuration.GetSection(CONNECTION_STRING_NAME).Value));

            services.AddDbContext<TTContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<DbContext, SW_InsideContext>();
            services.AddScoped<DbContext, TTContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<ITrackableRepository<Operator>, TrackableRepository<Operator>>();
            //services.AddScoped<ITTService<Operator>, TTService<Operator>>();

            services.AddScoped<ITrackableRepository<Users>, TrackableRepository<Users>>();
            services.AddScoped<ITTService<Users>, TTService<Users>>();

            services.AddScoped<ITrackableRepository<Roles>, TrackableRepository<Roles>>();
            services.AddScoped<ITTService<Roles>, TTService<Roles>>();

            services.AddScoped<ITrackableRepository<UserRoles>, TrackableRepository<UserRoles>>();
            services.AddScoped<ITTService<UserRoles>, TTService<UserRoles>>();

            services.AddHttpClient("LoginClient", option =>
                option.BaseAddress = new System.Uri(Configuration.GetSection(API_BACK_END).Value)
                ); ;

            // [Asma Khalid]: Authorization settings.  
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/User/Login");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
                options.ReturnUrlParameter = "RequestPath";
                //options.Cookie = new CookieBuilder()
                //{
                //    HttpOnly = true,
                //    Name = ".aspNetCoreDemo.Security.Cookie",
                //    Path = "/",
                //    SameSite = SameSiteMode.Lax,
                //    SecurePolicy = CookieSecurePolicy.SameAsRequest
                //};
                //options.SlidingExpiration = true;
            });


            return services;
        }
    }
}