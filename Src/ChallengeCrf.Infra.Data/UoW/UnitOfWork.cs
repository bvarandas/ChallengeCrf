using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Infra.Data.Context;
using Microsoft.Extensions.Logging;

namespace ChallengeCrf.Infra.Data.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowContext _dbContext;
    private readonly ILogger<UnitOfWork> _logger;
    public UnitOfWork(CashFlowContext dbContext, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Commit(CancellationToken cancellationToken)
    {
        bool trySave = false;
        try
        {
            trySave = true;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            trySave = false;
        }
        return trySave;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
