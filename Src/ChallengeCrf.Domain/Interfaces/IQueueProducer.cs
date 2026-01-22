using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;

namespace ChallengeCrf.Domain.Interfaces;

public interface IQueueProducer
{
    Task PublishMessageAsync(EnvelopeMessage<CashFlow> message);
    Task PublishMessageAsync(EnvelopeMessage<DailyConsolidated> message);
}
