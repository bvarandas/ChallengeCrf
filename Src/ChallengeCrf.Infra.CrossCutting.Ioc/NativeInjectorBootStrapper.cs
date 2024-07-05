using Microsoft.Extensions.DependencyInjection;

using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Application.Services;
using MediatR;
using ChallengeCrf.Domain.Notifications;
using ChallengeCrf.Domain.EventHandlers;
using ChallengeCrf.Domain.Events;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Infra.CrossCutting.Bus;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.CommandHandlers;
using ChallengeCrf.Infra.Data.Repository;
using ChallengeCrf.Infra.Data.UoW;
using ChallengeCrf.Infra.Data.Context;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;

using ChallengeCrf.Infra.Data.EventSourcing;
using ChallengeCrf.Appplication.Interfaces;
using FluentResults;
using ChallengeCrf.Infra.CrossCutting.Middleware;

namespace ChallengeCrf.Infra.CrossCutting.Ioc;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Domain Bus (Mediator)
        services.AddSingleton<IMediatorHandler, InMemoryBus>();

        // Middleware
        //services.AddSingleton<ConfigurationGlobalException>(options => options.);
        //services.AddSingleton<ConfigurationGlobalException>(opt => opt.);

        // Application
        services.AddSingleton<ICashFlowService, CashFlowService>();
        services.AddSingleton<IDailyConsolidatedService, DailyConsolidatedService>();

        // Domain - Events
        services.AddSingleton<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddSingleton<INotificationHandler<CashFlowInsertedEvent>, RegisterEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowUpdatedEvent>, RegisterEventHandler>();
        services.AddSingleton<INotificationHandler<CashFlowRemovedEvent>, RegisterEventHandler>();

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