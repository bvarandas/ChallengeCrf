using ChallengeCrf.Domain.Events;

namespace ChallengeCrf.Infra.Data.Repository.EventSourcing;
public interface IEventStoreRepository : IDisposable
{
    void Store(StoredEvent theEvent);
    IList<StoredEvent> All(int aggregateId);
}