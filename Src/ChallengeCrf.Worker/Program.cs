using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Extesions;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Infra.CrossCutting.Bus;
using ChallengeCrf.Infra.CrossCutting.Ioc;
using ChallengeCrf.Infra.Data.Repository;
using ChallengeCrf.Queue.Worker.Configurations;
using ChallengeCrf.Queue.Worker.Workers;
using Common.Logging;
using Common.Logging.Correlation;
using MediatR;
using MongoFramework;
using Serilog;
using System.Reflection;


var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(builder =>
        {
            builder.Sources.Clear();
            builder.AddConfiguration(config);
        })
        .UseSerilog(Logging.ConfigureLogger)
        .ConfigureServices(services =>
        {
            services.AddAppConfiguration(config);

            services.AddSingleton<IWorkerProducer, WorkerProducer>();
            services.AddSingleton<ICashFlowService, CashFlowService>();
            services.AddSingleton<IDailyConsolidatedService, DailyConsolidatedService>();
            services.AddSingleton<IOutboxCache, OutboxCache>();
            services.AddSingleton<ICashFlowRepository, CashFlowRepository>();
            services.AddSingleton<IQueueProducer, QueueProducer>();

            //services.AddSingleton<IWorkerConsumer, WorkerConsumer>();

            services.AddHostedService<WorkerMessage>();
            services.AddHostedService<WorkerConsumer>();
            services.AddHostedService<WorkerDailyConsolidated>();

            services.Configure<CashFlowSettings>(config.GetSection("CashFlowStoreDatabase"));
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            services.AddTransient<IMongoDbConnection>((provider) =>
            {
                var urlMongo = new MongoDB.Driver.MongoUrl("mongodb://root:example@mongo:27017/challengeCrf?authSource=admin");

                return MongoDbConnection.FromUrl(urlMongo);
            });

            services.AddAutoMapperSetup();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            NativeInjectorBootStrapper.RegisterServices(services, config);

        }).Build();


await host
.RunAsync();