using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Handlers;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.EventHandlers;
using ChallengeCrf.Domain.Events;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Notifications;
using ChallengeCrf.Infra.CrossCutting.Bus;
using ChallengeCrf.Infra.Data;
using ChallengeCrf.Infra.Data.Context;
using ChallengeCrf.Infra.Data.EventSourcing;
using ChallengeCrf.Infra.Data.Repository;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using ChallengeCrf.Infra.Data.UoW;
using ChallengeCrf.Queue.Worker.Configurations;
using Common.Logging.Correlation;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
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
        services.AddSingleton<IDailyConsolidatedService, DailyConsolidatedService>();

        // Domain - Commands
        services.AddSingleton<IRequestHandler<InsertCashFlowCache, Result<bool>>, CashFlowCacheHandler>();
        services.AddSingleton<IRequestHandler<UpdateCashFlowCache, Result<bool>>, CashFlowCacheHandler>();
        services.AddSingleton<IRequestHandler<RemoveCashFlowCache, Result<bool>>, CashFlowCacheHandler>();

        // Domain - Events
        services.AddSingleton<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddSingleton<INotificationHandler<CashFlowInsertedEvent>, CashFlowEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowUpdatedEvent>, CashFlowEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowRemovedEvent>, CashFlowEventHandler>();

        //Infra
        services.AddSingleton<IOutboxCache, OutboxCache>();
        services.AddSingleton<ICashFlowRepository, CashFlowRepository>();
        services.AddSingleton<IDailyConsolidatedRepository, DailyConsolidatedRepository>();

        services.AddSingleton<CashFlowContext>();
        services.AddSingleton<UserContext>();

        //DragonflyDB settings
        services.Configure<ConnectionDragonflyDB>(config.GetSection(nameof(ConnectionDragonflyDB)));

        //SignalR
        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Append("application/octet-stream");
        });
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
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}
