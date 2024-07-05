using ChallengeCrf.Application.ViewModel;
namespace ChallengeCrf.Application.Interfaces;
public interface IWorkerProducer
{
    //Task PublishMessage(CashFlow message);
    Task PublishMessages(List<CashFlowViewModel> message);
}
