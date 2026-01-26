using ChallengeCrf.Domain.Models;
using FluentResults;
namespace ChallengeCrf.Domain.Interfaces;
public interface IOutboxCache
{
    public Task<Result> UpsertCashflowAsync(OutboxMessage outboxMessage, string cashFlowIdTemp);
    public Task<Result> RemoveCashflowAsync(CashFlow cash);
    public Task<Result<IEnumerable<OutboxMessage>>> GetCashFlowOutboxAsync();
}
