using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tastee.Application.Interfaces;
using Tastee.Infrastucture.Data.Context;
using Tastee.Services;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<tasteeContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TasteeConnection")));

            services.AddScoped<DbContext, tasteeContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<Operators>, TrackableRepository<Operators>>();
            services.AddScoped<ITasteeService<Operators>, TasteeService<Operators>>();

            services.AddScoped<ITrackableRepository<Roles>, TrackableRepository<Roles>>();
            services.AddScoped<ITasteeService<Roles>, TasteeService<Roles>>();

            services.AddScoped<ITrackableRepository<OperatorRoles>, TrackableRepository<OperatorRoles>>();
            services.AddScoped<ITasteeService<OperatorRoles>, TasteeService<OperatorRoles>>();

            services.AddScoped<ITrackableRepository<Users>, TrackableRepository<Users>>();
            services.AddScoped<ITasteeService<Users>, TasteeService<Users>>();

            services.AddScoped<ITrackableRepository<Brands>, TrackableRepository<Brands>>();
            services.AddScoped<ITasteeService<Brands>, TasteeService<Brands>>();

        }
    }
}
