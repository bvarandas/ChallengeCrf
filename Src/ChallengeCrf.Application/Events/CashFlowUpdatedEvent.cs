using ChallengeCrf.Domain.Bus;

namespace ChallengeCrf.Domain.Events;

public class CashFlowUpdatedEvent : Event
{
    public string CashFlowId { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string Entry { get; set; }
    public DateTime Date { get; set; }
    public string Action { get; set; } = string.Empty;
    public CashFlowUpdatedEvent(string cashFlowId, string description, double amount, string entry, DateTime date, string action)
    {
        CashFlowId = cashFlowId;
        Description = description;
        Entry = entry;
        Amount = amount;
        Date = date;
        Action = action;
    }
}
