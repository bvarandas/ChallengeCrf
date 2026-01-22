using ChallengeCrf.Application.Commands;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Notifications;
using ChallengeCrf.Domain.ValueObjects;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ChallengeCrf.Tests")]
namespace ChallengeCrf.Application.CommandHandlers;
public sealed class CashFlowCommandHandler : CommandHandler,
    IRequestHandler<InsertCashFlowCommand, Result<bool>>,
    IRequestHandler<UpdateCashFlowCommand, Result<bool>>,
    IRequestHandler<RemoveCashFlowCommand, Result<bool>>
{
    private readonly ICashFlowRepository _registerRepository;
    private readonly IOutboxCache _outboxRepository;
    private readonly IMediatorHandler _mediator;
    private readonly IQueueProducer _queueProducer;
    private readonly ILogger<CashFlowCommandHandler> _logger;
    public CashFlowCommandHandler(
        ICashFlowRepository registerRepository,
        IUnitOfWork uow,
        IMediatorHandler mediator,
        INotificationHandler<DomainNotification> notifications,
        IOutboxCache outboxRepository,
        IQueueProducer queueProducer,
        ILogger<CashFlowCommandHandler> logger) : base(uow, mediator, notifications)
    {
        _registerRepository = registerRepository;
        _mediator = mediator;
        _outboxRepository = outboxRepository;
        _queueProducer = queueProducer;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(InsertCashFlowCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            NotifyValidationErrors(command);
            return await Task.FromResult(false);
        }

        var cashFlow = new CashFlow(command.Description, command.Amount, command.Entry, command.Date);

        try
        {
            await _outboxRepository.UpsertCashflowAsync(cashFlow); /// insere no outbox.

            var envelopeMessage = new EnvelopeMessage<CashFlow>(cashFlow);
            envelopeMessage.Action = UserAction.Insert;

            await _queueProducer.PublishMessageAsync(envelopeMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return await Task.FromResult<Result<bool>>(false);
        }

        return await Task.FromResult(true);
    }

    public async Task<Result<bool>> Handle(UpdateCashFlowCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            NotifyValidationErrors(command);
            return await Task.FromResult(false);
        }
        var cashFlow = new CashFlow(command.CashFlowId, command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        await _outboxRepository.UpsertCashflowAsync(cashFlow);

        //await _registerRepository.UpdateCashFlowAsync(cashFlow);

        //if (await Commit(cancellationToken))
        //{
        //    await _mediator.RaiseEvent(new CashFlowUpdatedEvent(cashFlow.CashFlowId, cashFlow.Description, cashFlow.Amount, cashFlow.Entry, cashFlow.Date));
        //}

        return await Task.FromResult(true);
    }

    public async Task<Result<bool>> Handle(RemoveCashFlowCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            NotifyValidationErrors(command);
            return await Task.FromResult(false);
        }

        var cashFlow = new CashFlow(command.CashFlowId, command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        await _outboxRepository.RemoveCashflowAsync(cashFlow);

        //_registerRepository.DeleteCashFlowAsync(command.CashFlowId);

        //if (await Commit(cancellationToken))
        //{
        //    await _mediator.RaiseEvent(new CashFlowRemovedEvent(command.CashFlowId));
        //}

        return await Task.FromResult(true);
    }
    public void Dispose()
    {
        _registerRepository.Dispose();
    }
}
