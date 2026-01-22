using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Notifications;
using MediatR;
using System.Threading;

namespace ChallengeCrf.Application.CommandHandlers;

public class CommandHandler
{
    private readonly IUnitOfWork _uow;
    private readonly IMediatorHandler _mediator;
    private readonly DomainNotificationHandler _notifications;

    public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
    {
        _uow = uow;
        _notifications = (DomainNotificationHandler)notifications;
        _mediator = bus;
    }

    protected void NotifyValidationErrors(Command message)
    {
        foreach (var error in message.ValidationResult.Errors)
        {
            _mediator.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
        }
    }

    public async Task<bool> Commit(CancellationToken cancelationToken )
    {
        if (_notifications.HasNotifications()) return false;
        if (await _uow.Commit(cancelationToken)) return true;

        await _mediator.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
        return false;
    }
}
