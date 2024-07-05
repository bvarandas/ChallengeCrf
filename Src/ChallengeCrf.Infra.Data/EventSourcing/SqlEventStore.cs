using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Events;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using Newtonsoft.Json;

namespace ChallengeCrf.Infra.Data.EventSourcing;
public class SqlEventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    public SqlEventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public void Save<T>(T theEvent) where T : Event
    {
        var serializedData = JsonConvert.SerializeObject(theEvent);

        var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                "bvarandas");

        _eventStoreRepository.Store(storedEvent);
    }
}
