using ChallengeCrf.Domain.Extesions;
using ChallengeCrf.Queue.Worker.Workers;
using ChallengeCrf.Queue.Worker.Configurations;
using System.Reflection;
using MediatR;
using MongoFramework;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Infra.CrossCutting.Ioc;
using Serilog;
using Common.Logging;
using Common.Logging.Correlation;


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

            NativeInjectorBootStrapper.RegisterServices(services);

        }).Build();


    await host
    .RunAsync();