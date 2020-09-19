using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Tastee.Models.SqlDataContext;
using Tastee.Models.SqlDataContext;
using Tastee.Services;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace Tastee
{
    public static class ExtendConfig
    {
        const string CONNECTION_STRING_NAME = "DbConnectString";
        public static IServiceCollection InsideConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddDbContext<TTContext>(options =>
            //options.UseNpgsql(Configuration.GetSection(CONNECTION_STRING_NAME).Value));

            services.AddDbContext<tas77143_tasteeContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TasteeConnection")));

            services.AddScoped<DbContext, tas77143_tasteeContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<Operators>, TrackableRepository<Operators>>();
            services.AddScoped<ITTService<Operators>, TTService<Operators>>();

            services.AddScoped<ITrackableRepository<Roles>, TrackableRepository<Roles>>();
            services.AddScoped<ITTService<Roles>, TTService<Roles>>();

            services.AddScoped<ITrackableRepository<OperatorRoles>, TrackableRepository<OperatorRoles>>();
            services.AddScoped<ITTService<OperatorRoles>, TTService<OperatorRoles>>();

            services.AddScoped<ITrackableRepository<Users>, TrackableRepository<Users>>();
            services.AddScoped<ITTService<Users>, TTService<Users>>();

            services.AddScoped<ITrackableRepository<Brands>, TrackableRepository<Brands>>();
            services.AddScoped<ITTService<Brands>, TTService<Brands>>();

            return services;
        }
    }
}