using ChallengeCrf.Domain.Bus;
namespace ChallengeCrf.Domain.Events;
public class CashFlowRemovedEvent : Event
{
    public string CashFlowId { get; set; }
    public CashFlowRemovedEvent(string cashFlowId)
    {
        CashFlowId = cashFlowId;
    }
}