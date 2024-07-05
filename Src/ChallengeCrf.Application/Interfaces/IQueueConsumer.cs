using ChallengeCrf.Application.ViewModel;

namespace ChallengeCrf.Application.Interfaces;

public interface IQueueConsumer
{
    CashFlowViewModel RegisterGetById(string cashFlowId);
}
