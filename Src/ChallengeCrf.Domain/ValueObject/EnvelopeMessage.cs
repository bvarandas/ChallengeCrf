using ProtoBuf;
namespace ChallengeCrf.Domain.ValueObjects;
[ProtoContract()]
public class EnvelopeMessage<T> where T: class
{
    [ProtoMember(1)]
    public T Body { get; private set; } = null!;

    [ProtoMember(2)]
    public DateTime LastTransaction { get; set; } = DateTime.Now;

    [ProtoMember(3)]
    public string Action { get; set; } = string.Empty;

    public EnvelopeMessage( T body)
    {
        Body = body;
    }

    public EnvelopeMessage()
    {

    }
}