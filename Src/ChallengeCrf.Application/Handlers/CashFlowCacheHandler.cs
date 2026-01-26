using ChallengeCrf.Application.Commands;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Notifications;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("ChallengeCrf.Tests")]
namespace ChallengeCrf.Application.Handlers;

public class CashFlowCacheHandler : CommandHandler,
IRequestHandler<InsertCashFlowCache, Result<bool>>,
IRequestHandler<UpdateCashFlowCache, Result<bool>>,
IRequestHandler<RemoveCashFlowCache, Result<bool>>
{

    private readonly IOutboxCache _outboxRepository;
    private readonly ILogger<CashFlowCacheHandler> _logger;
    public CashFlowCacheHandler(
        IOutboxCache outboxRepository,
        IUnitOfWork uow,
        IMediatorHandler bus,
        ILogger<CashFlowCacheHandler> logger,
        INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
    {
        _outboxRepository = outboxRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(InsertCashFlowCache command, CancellationToken cancellationToken)
    {
        var cashFlow = new CashFlow(command.CashFlowIdTemp, command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        var outbox = await GetOutboxAsync(cashFlow, UserAction.Insert);

        var result = await _outboxRepository.UpsertCashflowAsync(outbox, command.CashFlowIdTemp);

        if (!result.IsSuccess)
        {
            _logger.LogError("Algum erro ao inserir o cash flow do cache");

            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }

    public async Task<Result<bool>> Handle(UpdateCashFlowCache command, CancellationToken cancellationToken)
    {
        var cashFlow = new CashFlow(command.CashFlowId, command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        var outbox = await GetOutboxAsync(cashFlow, UserAction.Update);

        var result = await _outboxRepository.UpsertCashflowAsync(outbox, cashFlow.CashFlowIdTemp);

        if (!result.IsSuccess)
        {
            _logger.LogError("Algum erro ao atualizar o cash flow do cache");

            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }

    public async Task<Result<bool>> Handle(RemoveCashFlowCache command, CancellationToken cancellationToken)
    {
        var cashFlow = new CashFlow(command.CashFlowId, command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        var outbox = await GetOutboxAsync(cashFlow, UserAction.Delete);

        var result = await _outboxRepository.UpsertCashflowAsync(outbox, command.CashFlowIdTemp);

        if (!result.IsSuccess)
        {
            _logger.LogError("Algum erro ao remover o cash flow do cache");

            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }

    private async Task<OutboxMessage> GetOutboxAsync(CashFlow cashFlow, string action)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = cashFlow.GetType().FullName,
            Content = JsonSerializer.Serialize(cashFlow),
            OccurredOnUtc = DateTime.UtcNow,
            UserAction = action
        };
    }
}
