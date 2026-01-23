using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Events;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.CrossCutting.Bus;
using ChallengeCrf.Infra.Data;
using ChallengeCrf.Infra.Data.Context;
using ChallengeCrf.Infra.Data.EventSourcing;
using ChallengeCrf.Infra.Data.Repository;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using ChallengeCrf.Queue.Worker.Configurations;
using Common.Logging.Correlation;
using MongoFramework;
using System.Reflection;

namespace ChallengeCrf.Api.Configurations;

internal class NativeInjectorBoostrapper
{
    public static void RegisterServices(IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<QueueCommandSettings>(config.GetSection(nameof(QueueCommandSettings)));
        services.Configure<QueueEventSettings>(config.GetSection(nameof(QueueEventSettings)));
        services.AddSingleton<IQueueConsumer, QueueConsumer>();
        services.AddSingleton<IQueueProducer, QueueProducer>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();


        // Domain Bus (Mediator)
        services.AddSingleton<IMediatorHandler, InMemoryBus>();

        //Application
        services.AddSingleton<ICashFlowService, CashFlowService>();

        //Infra
        services.AddSingleton<IOutboxCache, OutboxCache>();
        services.AddSingleton<ICashFlowRepository, CashFlowRepository>();
        services.AddSingleton<CashFlowContext>();

        //DragonflyDB settings
        services.Configure<ConnectionDragonflyDB>(config.GetSection(nameof(ConnectionDragonflyDB)));

        //SignalR
        services.AddSignalR();

        // automapper
        services.AddAutoMapperSetup();

        // Asp .NET HttpContext dependency
        services.AddHttpContextAccessor();

        // Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        services.AddCors(options => options.AddPolicy("CorsPolicy", builderc =>
        {
            builderc
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://locallhost:4200")
            .AllowCredentials();
            //.SetIsOriginAllowed((host) => true)

        }));

        services.AddTransient<IMongoDbConnection>((provider) =>
        {
            var urlMongo = new MongoDB.Driver.MongoUrl("mongodb://root:example@mongo:27017/challengeCrf?authSource=admin");

            return MongoDbConnection.FromUrl(urlMongo);
        });

        services.AddControllers();

        //services.AddSingleton<IQueueConsumer, QueueConsumer>();
        services.AddHostedService<QueueConsumer>();
        services.AddHostedService<QueueProducer>();
        services.AddHostedService<OutboxProcessorService>();

        // Infra - Data EventSourcing
        services.AddSingleton<IEventStoreRepository, EventStoreSQLRepository>();
        services.AddSingleton<IEventStore, SqlEventStore>();
        services.AddSingleton<EventStoreSqlContext>();
    }
}
