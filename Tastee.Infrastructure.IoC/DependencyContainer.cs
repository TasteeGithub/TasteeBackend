using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
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
            services.AddScoped<IGenericService<Operators>, GenericService<Operators>>();

            services.AddScoped<ITrackableRepository<Roles>, TrackableRepository<Roles>>();
            services.AddScoped<IGenericService<Roles>, GenericService<Roles>>();

            services.AddScoped<ITrackableRepository<OperatorRoles>, TrackableRepository<OperatorRoles>>();
            services.AddScoped<IGenericService<OperatorRoles>, GenericService<OperatorRoles>>();

            services.AddScoped<ITrackableRepository<Users>, TrackableRepository<Users>>();
            services.AddScoped<IGenericService<Users>, GenericService<Users>>();

            services.AddScoped<ITrackableRepository<Brands>, TrackableRepository<Brands>>();
            services.AddScoped<ITrackableRepository<RestaurantSpace>, TrackableRepository<RestaurantSpace>>();
            services.AddScoped<IGenericService<Brands>, GenericService<Brands>>();
            services.AddScoped<IGenericService<RestaurantSpace>, GenericService<RestaurantSpace>>();
            services.AddScoped<IBrandService, BrandService>();

            services.AddScoped<ITrackableRepository<Cities>, TrackableRepository<Cities>>();
            services.AddScoped<IGenericService<Cities>, GenericService<Cities>>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<ITrackableRepository<Banners>, TrackableRepository<Banners>>();
            services.AddScoped<IGenericService<Banners>, GenericService<Banners>>();
            services.AddScoped<IBannerService, BannerService>();

            services.AddScoped<ITrackableRepository<ProductSliders>, TrackableRepository<ProductSliders>>();
            services.AddScoped<IGenericService<ProductSliders>, GenericService<ProductSliders>>();
            services.AddScoped<IProductSliderService, ProductSliderService>();

            services.AddScoped<ITrackableRepository<Areas>, TrackableRepository<Areas>>();
            services.AddScoped<IGenericService<Areas>, GenericService<Areas>>();
            services.AddScoped<IAreaService, AreaService>();

            services.AddScoped<IFileService, FileService>();

            services.AddMediatR(System.AppDomain.CurrentDomain.GetAssemblies());

        }
    }
}