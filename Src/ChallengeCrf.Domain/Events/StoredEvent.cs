using ChallengeCrf.Domain.Bus;
using System.ComponentModel.DataAnnotations;

namespace ChallengeCrf.Domain.Events;
public class StoredEvent : Event
{
    [Key]
    public Guid Id { get; private set; }

    public string Data { get; private set; }

    public string User { get; private set; }

    public StoredEvent(Event theEvent, string data, string user) 
    {
        Id = Guid.NewGuid();
        AggregateId = theEvent.AggregateId;
        MessageType = theEvent.MessageType;
        Data = data;
        User = user;
    }

}
