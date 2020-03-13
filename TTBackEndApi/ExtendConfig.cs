using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace TTBackEndApi
{
    public static class ExtendConfig
    {
        const string CONNECTION_STRING_NAME = "DbConnectString";
        public static IServiceCollection InsideConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<SW_InsideContext>(options =>
            options.UseNpgsql(Configuration.GetSection(CONNECTION_STRING_NAME).Value));

            services.AddScoped<DbContext, SW_InsideContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<Operator>, TrackableRepository<Operator>>();
            services.AddScoped<ITTService<Operator>, TTService<Operator>>();


            return services;
        }
    }
}