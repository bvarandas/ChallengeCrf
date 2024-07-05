using ChallengeCrf.Domain.Bus;

namespace ChallengeCrf.Domain.Events;
public  class CashFlowInsertedEvent : Event
{
    public string CashFlowId { get; set; } 
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string Entry{ get; set; }
    public DateTime Date { get; set; }
    public string Action { get; set; } = string.Empty;

    public CashFlowInsertedEvent(string cashFlowId, string description,double cashValue, string entry, DateTime date, string action)
    {
        CashFlowId = cashFlowId;
        Description = description;
        Amount = cashValue; 
        Entry = entry;
        Date = date;
        Action = action;
    }
}