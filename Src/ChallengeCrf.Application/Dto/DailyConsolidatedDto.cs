namespace ChallengeCrf.Application.Dto;
public class DailyConsolidatedDto
{
    public int DailyConsolidatedId { get; set; }
    public double AmountCredit { get; set; }
    public double AmountDebit { get; set; }
    public DateTime Date { get; set; }
    public double AmoutTotal { get; set; }
    public IEnumerable<CashFlowDto> CashFlows { get; set; } = null!;
}
