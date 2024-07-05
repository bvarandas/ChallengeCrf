using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using ChallengeCrf.Infra.Data.Context;
using FluentResults;
using Microsoft.Extensions.Logging;
namespace ChallengeCrf.Infra.Data.Repository;
public class DailyConsolidatedRepository : IDailyConsolidatedRepository
{
    private readonly ILogger<DailyConsolidatedRepository> _logger;
    protected readonly CashFlowContext _dbContext;

    public DailyConsolidatedRepository(CashFlowContext dbContext, ILogger<DailyConsolidatedRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<IAsyncEnumerable<EnvelopeMessage<DailyConsolidated>>>> GetDailyConsolidatedListAllAsync()
    {
        IAsyncEnumerable<EnvelopeMessage<DailyConsolidated>> dailyConsolidatedList = null;
        try
        {
            _logger.LogInformation("Coletando  GetAllDailyConsolidatedAsync no MongoDB");

            var daily = _dbContext
                .DailyConsolidated
                .AsNoTracking()
                .ToAsyncEnumerable();

            _logger.LogInformation("Conseguiu Coletar do MongoDB em GetAllDailyConsolidatedAsync ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return Result.Ok(dailyConsolidatedList);

    }

    public async Task<Result<DailyConsolidated>> GetDailyConsolidatedByDateAsync(DateTime date)
    {
        var cashFlowResult = await _dbContext
            .DailyConsolidated
            .ToAsyncEnumerable()
            .SingleOrDefaultAsync(x => x.Date.ToString("yyyyMMdd") == date.ToString("yyyyMMdd"));

        return Result.Ok(cashFlowResult);
    }

    public async Task<Result<DailyConsolidated>> UpdateDailyConsolidatedAsync(DailyConsolidated dailyConsolidated)
    {
        try
        {
            _dbContext.DailyConsolidated.Update(dailyConsolidated);
            
            await _dbContext.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError($"{ex.Message}");
            Result.Fail(new Error(ex.Message));
        }

        return  Result.Ok(dailyConsolidated);
    }


    public async Task<Result<DailyConsolidated>> AddDailyConsolidatedAsync(DailyConsolidated dailyConsolidated)
    {
        _logger.LogInformation("Inserindo no banco de dados");
        try
        {
            _dbContext.DailyConsolidated.Add(dailyConsolidated);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Result.Fail(new Error(ex.Message));
        }
        return Result.Ok(dailyConsolidated);
    }
}
