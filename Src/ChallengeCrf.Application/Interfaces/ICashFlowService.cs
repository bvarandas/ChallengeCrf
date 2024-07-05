using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.ViewModel;
namespace ChallengeCrf.Application.Interfaces;
public interface ICashFlowService
{
    Task<IAsyncEnumerable<CashFlowViewModel>> GetListAllAsync();
    Task<CashFlowViewModel> GetCashFlowyIDAsync(string cashFlowId);
    Task AddCashFlowAsync(CashFlowCommand register);
    Task UpdateCashFlowAsync(CashFlowCommand register);
    IList<CashFlowHistoryData> GetAllHistory(int registerId);
    void RemoveCashFlowAsync(string cashFlowId);
}