using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTBackEndApi.Models.DataContext;
//using TTBackEndApi.Repositories;
using TTBackEndApi.Services;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Services;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;
using URF.Core.Services;

namespace TTBackEndApi
{
    public static class ExtendConfig
    {
        public static IServiceCollection InsideConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<DbContext, sw_insideContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<IswRequests>, TrackableRepository<IswRequests>>();
            //services.AddScoped<ITrackableRepository<IswRequestHistory>, TrackableRepository<IswRequestHistory>>();

            //services.AddScoped(typeof(IRepository<IswRequests>), typeof(Repository<IswRequests>));
            services.AddScoped<IRequestService, RequestService>();

            //services.AddScoped(typeof(IRepository<IswRequestHistory>), typeof(Repository<IswRequestHistory>));
            //services.AddScoped(typeof(IService<IswRequestHistory>), typeof(Service<IswRequestHistory>));

            //services.AddScoped(typeof(IRepository<IswHoldUnholdMapping>), typeof(Repository<IswHoldUnholdMapping>));
            //services.AddScoped(typeof(IService<IswHoldUnholdMapping>), typeof(Service<IswHoldUnholdMapping>));

            //services.AddScoped(typeof(IRepository<IswExportLogs>), typeof(Repository<IswExportLogs>));
            //services.AddScoped(typeof(IService<IswExportLogs>), typeof(Service<IswExportLogs>));

            //services.AddScoped<IApiRepository, ApiRepository>();
            //services.AddScoped<ITransactionApiService, TransactionApiService>();
            //services.AddScoped<ISellerWalletApiService, SellerWalletApiService>();
            //services.AddTransient<IServiceHelper, ServiceHelper>();
            //services.AddScoped<IProducerProcess, ProducerProcess>();
            //services.AddScoped<IRequestApiService, RequestApiService>();
            //services.AddScoped<IExportService, ExportService>();
            //services.AddScoped<IShopInfoApiService, ShopInfoApiService>();
            //services.AddScoped<IUploadService, UploadService>();

            //#region producer and consumer to kafka seller

            //var producerConfig = new ProducerConfig();
            //var consumerConfig = new ConsumerConfig();
            //Configuration.Bind("sw_producer", producerConfig);
            //Configuration.Bind("sw_consumer", consumerConfig);
            //services.AddSingleton(producerConfig);
            //services.AddSingleton(consumerConfig);

            //#endregion producer and consumer to kafka seller

            //#region Config for Job

            ////---------------------
            //services.AddScoped<UnHoldJob>();
            //services.AddScoped<ExportJob>();

            //services.AddSingleton<IJobFactory, JobFactory>();
            //var serviceProvider = services.BuildServiceProvider();

            //ScheduleJob(serviceProvider);

            //#endregion Config for Job

            //services.AddScoped<IPolicyEvaluator, CustomPolicyEvaluator>();

            //services.AddTransient<IProducerProcess, ProducerProcess>();
            //services.AddTransient<IConsumerProcess, ConsumerProcess>();
            ////services.AddTransient<IConsumerProcessFailed, ConsumerProcessFailed>();

            ////services.AddHostedService<ConsumerRealtimeFaild>();
            //services.AddHostedService<ConsumerRealtimeSuccess>();


            //services.AddHttpContextAccessor();
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            return services;
        }
    }
}
