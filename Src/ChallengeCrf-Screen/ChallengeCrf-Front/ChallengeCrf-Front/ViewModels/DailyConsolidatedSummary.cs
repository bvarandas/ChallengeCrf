namespace ChallengeCrf_Front.ViewModels;

public class DailyConsolidatedSummary
{
    public List<CashFlowSummary> cashFlows { get; set; }
    public decimal amountDebit { get; set; }
    public decimal amountCredit { get; set; }
    public decimal amountTotal { get; set; }
    public DateTime date { get; set; }
}
