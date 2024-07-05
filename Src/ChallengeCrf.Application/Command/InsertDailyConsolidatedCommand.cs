using ChallengeCrf.Aplication.Commands;

namespace ChallengeCrf.Application.Commands;

public class InsertDailyConsolidatedCommand : DailyConsolidatedCommand
{
    protected InsertDailyConsolidatedCommand(double amountCredit, double amountDebit, DateTime date, string action)
    {
        AmountCredit = amountCredit;
        AmountDebit = amountDebit;
        Date = date;
        Action = action;
    }

    public override bool IsValid()
    {
        return true;
    }
}
