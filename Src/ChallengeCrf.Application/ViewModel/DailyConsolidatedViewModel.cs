namespace ChallengeCrf.Application.ViewModel;
public class DailyConsolidatedViewModel
{
    public int DailyConsolidatedId { get; set; }
    public double AmountCredit { get; set; }
    public double AmountDebit { get; set; }
    public DateTime Date { get; set; }
    public double AmoutTotal { get; set; }
    public IEnumerable<CashFlowViewModel> CashFlows { get; set; } = null!;
}
