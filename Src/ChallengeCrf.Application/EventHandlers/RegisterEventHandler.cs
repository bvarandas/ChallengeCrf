using ChallengeCrf.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeCrf.Domain.EventHandlers;

public class RegisterEventHandler :
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
