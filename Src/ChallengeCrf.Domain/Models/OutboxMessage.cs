using ProtoBuf;

namespace ChallengeCrf.Domain.Models
{
    [ProtoContract]
    public class OutboxMessage
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public string Type { get; set; } = default!;

        [ProtoMember(3)]
        public string Content { get; set; } = default!;

        [ProtoMember(4)]
        public DateTime OccurredOnUtc { get; set; }

        [ProtoMember(5)]
        public DateTime? ProcessedOnUtc { get; set; }

        [ProtoMember(6)]
        public string? Error { get; set; }

        [ProtoMember(7)]
        public string UserAction { get; set; }

        public OutboxMessage() { }

        public void MarkAsProcessed()
        {
            ProcessedOnUtc = DateTime.UtcNow;
        }

        public void SetError(string errorMessage)
        {
            Error = errorMessage;
        }
    }
}
