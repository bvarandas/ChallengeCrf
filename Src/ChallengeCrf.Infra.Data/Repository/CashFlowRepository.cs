using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Infra.Data.Context;
using MongoFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentResults;
using ChallengeCrf.Domain.ValueObjects;

namespace ChallengeCrf.Infra.Data.Repository;
public class CashFlowRepository: ICashFlowRepository
{
    private readonly ILogger<CashFlowRepository> _logger;
    protected readonly CashFlowContext _dbContext;
    public CashFlowRepository(CashFlowContext dbContext, ILogger<CashFlowRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<Result> AddCashFlowAsync(CashFlow cashFlow)
    {
        _logger.LogInformation("Inserindo de CashFlow no banco de dados");
        
        try
        {
             _dbContext.CashFlow.Add(cashFlow);
            
        }catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return Result.Fail(new Error(ex.Message));
        }

        return Result.Ok();
    }

    public async Task<Result> DeleteCashFlowAsync(string cashFlowId)
    {
        try
        {
            var filtered = await _dbContext
                .CashFlow
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.CashFlowId == cashFlowId);
            _dbContext.CashFlow.Remove(filtered);
        }catch (Exception ex) {
            _logger.LogError(ex.Message);
            return Result.Fail(new Error(ex.Message));
        }
        return Result.Ok();
    }
    public async Task<Result<IAsyncEnumerable<CashFlow>>> GetAllCashFlowAsync()
    {
        IAsyncEnumerable<CashFlow>? registerList = null;
        try
        {
            _logger.LogInformation("Coletando  GetAllCashFlowAsync no MongoDB");
            
            registerList =  _dbContext.CashFlow
                .AsNoTracking()
                .ToAsyncEnumerable();

            _logger.LogInformation("Conseguiu Coletar do MongoDB em GetAllCashFlowAsync ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Fail(new Error(ex.Message));
        }
        return Result.Ok(registerList);
    }
    public async Task<Result<CashFlow>> GetCashFlowByIDAsync(string cashFlowId)
    {
        var registerResult = await _dbContext
            .CashFlow
            .ToAsyncEnumerable()
            .SingleOrDefaultAsync(x => x.CashFlowId == cashFlowId);

        return Result.Ok(registerResult);
    }

    public async Task<Result<IAsyncEnumerable<CashFlow>>> GetCashFlowByDateAsync(DateTime date)
    {
        var registerResult = _dbContext
            .CashFlow
            .AsNoTracking()
            .ToAsyncEnumerable()
            .Where(x => x.Date.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"));
        
        return Result.Ok(registerResult);
    }

    public async Task<Result> UpdateCashFlowAsync(CashFlow cashFlow)
    {
        try
        {
            var local = _dbContext.CashFlow.
                AsNoTracking().
                FirstOrDefault(entry =>
                    entry
                    .CashFlowId
                    .Equals(cashFlow.CashFlowId));

            _dbContext.CashFlow.Update(cashFlow);
        }catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Fail(new Error(ex.Message));
        }
        return Result.Ok();
    }
    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }

}