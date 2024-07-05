using ChallengeCrf.Domain.Bus;
namespace ChallengeCrf.Domain.Events;

public interface IEventStore
{
    void Save<T>(T theEvent) where T : Event;
}
