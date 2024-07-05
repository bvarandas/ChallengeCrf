using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using FluentResults;
namespace ChallengeCrf.Domain.Interfaces;
public interface ICashFlowRepository : IDisposable
{
    public Task<Result<CashFlow>> GetCashFlowByIDAsync(string cashFlowId);
    public Task<Result<IAsyncEnumerable<CashFlow>>> GetCashFlowByDateAsync(DateTime date);
    public Task<Result> AddCashFlowAsync(CashFlow cashFlow);
    public Task<Result> UpdateCashFlowAsync(CashFlow cashFlow);
    public Task<Result<IAsyncEnumerable<CashFlow>>> GetAllCashFlowAsync();
    public  Task<Result> DeleteCashFlowAsync(string cashFlowId);
}