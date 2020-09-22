﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddDbContext<TasteeContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TasteeConnection")));

            services.AddScoped<DbContext, TasteeContext>();
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

            return services;
        }
    }
}