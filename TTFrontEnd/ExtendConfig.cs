using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TTFrontEnd.Models.DataContext;
//using TTFrontEnd.Models.SqlDataContext;
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
        public static IServiceCollection InsideConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<TTContext>(options =>
            options.UseNpgsql(Configuration.GetSection(CONNECTION_STRING_NAME).Value));

            //services.AddDbContext<TTContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DbContext, TTContext>();
            //services.AddScoped<DbContext, TTContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<ITrackableRepository<Operator>, TrackableRepository<Operator>>();
            //services.AddScoped<ITTService<Operator>, TTService<Operator>>();

            services.AddScoped<ITrackableRepository<Operators>, TrackableRepository<Operators>>();
            services.AddScoped<ITTService<Operators>, TTService<Operators>>();

            services.AddScoped<ITrackableRepository<Roles>, TrackableRepository<Roles>>();
            services.AddScoped<ITTService<Roles>, TTService<Roles>>();

            services.AddScoped<ITrackableRepository<OperatorRoles>, TrackableRepository<OperatorRoles>>();
            services.AddScoped<ITTService<OperatorRoles>, TTService<OperatorRoles>>();



            return services;
        }
    }
}