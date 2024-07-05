using ChallengeCrf.Domain.Bus;
namespace ChallengeCrf.Aplication.Commands;
public abstract class DailyConsolidatedCommand : Command
{
    public int DailyConsolidatedId { get; protected set; }
    public double AmountCredit { get; protected set; }
    public double AmountDebit { get; protected set; }
    public DateTime Date {  get; protected set; }
    public string Action {  get; protected set; }=string.Empty;
}
