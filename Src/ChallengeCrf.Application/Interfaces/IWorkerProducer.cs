using ChallengeCrf.Application.Dto;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Application.Interfaces;
public interface IWorkerProducer
{
    public Task PublishMessages(List<CashFlowDto> messageList);
    public Task PublishMessages(DailyConsolidated message);
}
