using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Dto;
using ChallengeCrf.Application.ViewModel;
namespace ChallengeCrf.Application.Interfaces;
public interface ICashFlowService
{
    Task<IAsyncEnumerable<CashFlowDto>> GetListAllAsync();
    Task<CashFlowDto> GetCashFlowyIDAsync(string cashFlowId);
    Task AddCashFlowAsync(CashFlowCommand register);
    Task UpdateCashFlowAsync(CashFlowCommand register);
    IList<CashFlowHistoryDto> GetAllHistory(int registerId);
    void RemoveCashFlowAsync(string cashFlowId);
}