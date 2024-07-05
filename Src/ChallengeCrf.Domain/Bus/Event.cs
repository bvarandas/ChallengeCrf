using MediatR;
namespace ChallengeCrf.Domain.Bus;
public abstract class Event : Message, INotification
{
    public DateTime Timestamp { get; private set; }
    protected Event() 
    {
        Timestamp = DateTime.UtcNow;
    }
}
