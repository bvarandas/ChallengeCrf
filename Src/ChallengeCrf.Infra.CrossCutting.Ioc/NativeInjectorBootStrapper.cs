using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Handlers;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.EventHandlers;
using ChallengeCrf.Domain.Events;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Notifications;
using ChallengeCrf.Infra.CrossCutting.Bus;
using ChallengeCrf.Infra.Data;
using ChallengeCrf.Infra.Data.Context;
using ChallengeCrf.Infra.Data.EventSourcing;
using ChallengeCrf.Infra.Data.Repository;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using ChallengeCrf.Infra.Data.UoW;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChallengeCrf.Infra.CrossCutting.Ioc;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services, IConfiguration config)
    {
        // Domain Bus (Mediator)
        services.AddSingleton<IMediatorHandler, InMemoryBus>();

        // Middleware
        //services.AddSingleton<ConfigurationGlobalException>(options => options.);
        //services.AddSingleton<ConfigurationGlobalException>(opt => opt.);

        //DragonflyDB settings
        services.Configure<ConnectionDragonflyDB>(config.GetSection(nameof(ConnectionDragonflyDB)));

        // Application
        services.AddSingleton<ICashFlowService, CashFlowService>();
        services.AddSingleton<IDailyConsolidatedService, DailyConsolidatedService>();

        // Domain - Events
        services.AddSingleton<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddSingleton<INotificationHandler<CashFlowInsertedEvent>, CashFlowEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowUpdatedEvent>, CashFlowEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowRemovedEvent>, CashFlowEventHandler>();

        // Domain - Commands
        services.AddSingleton<IRequestHandler<InsertCashFlowCommand, Result<bool>>, CashFlowCommandHandler>();
        services.AddSingleton<IRequestHandler<UpdateCashFlowCommand, Result<bool>>, CashFlowCommandHandler>();
        services.AddSingleton<IRequestHandler<RemoveCashFlowCommand, Result<bool>>, CashFlowCommandHandler>();

        // Infra - Data
        services.AddSingleton<ICashFlowRepository, CashFlowRepository>();
        services.AddSingleton<IDailyConsolidatedRepository, DailyConsolidatedRepository>();
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<CashFlowContext>();

        // Infra - Data EventSourcing
        services.AddSingleton<IEventStoreRepository, EventStoreSQLRepository>();
        services.AddSingleton<IEventStore, SqlEventStore>();
        services.AddSingleton<EventStoreSqlContext>();
    }
}