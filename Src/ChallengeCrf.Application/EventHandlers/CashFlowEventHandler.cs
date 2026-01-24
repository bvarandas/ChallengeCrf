using ChallengeCrf.Domain.Events;
using MediatR;

namespace ChallengeCrf.Domain.EventHandlers;

public class CashFlowEventHandler :
    INotificationHandler<CashFlowInsertedEvent>,
    INotificationHandler<CashFlowRemovedEvent>,
    INotificationHandler<CashFlowUpdatedEvent>
{
    public Task Handle(CashFlowInsertedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(CashFlowRemovedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(CashFlowUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
