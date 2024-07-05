namespace ChallengeCrf.Domain.Models
{
    public abstract class QueueSettings
    {
        public string HostName { get; set; } = string.Empty;
        public string QueueNameCashFlow { get; set; } = string.Empty;
        public string QueueNameDailyConsolidated { get; set; } = string.Empty;
        public string ExchangeService {  get; set; } = string.Empty;
        public string ExchangeType {  get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public int Port { get; set; }
        public ushort Interval { get; set; }
        
    }

    public class QueueCommandSettings : QueueSettings { }
    public class QueueEventSettings : QueueSettings { }
}
