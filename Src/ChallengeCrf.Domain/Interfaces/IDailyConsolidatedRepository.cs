using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using FluentResults;

namespace ChallengeCrf.Domain.Interfaces;
public interface IDailyConsolidatedRepository
{
    public Task<Result<DailyConsolidated>> GetDailyConsolidatedByDateAsync(DateTime date);
    public Task<Result<IAsyncEnumerable<EnvelopeMessage<DailyConsolidated>>>> GetDailyConsolidatedListAllAsync();
    public Task<Result<DailyConsolidated>> AddDailyConsolidatedAsync(DailyConsolidated dailyConsolidated);
    public Task<Result<DailyConsolidated>> UpdateDailyConsolidatedAsync(DailyConsolidated dailyConsolidated);
}