using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Validations;

namespace ChallengeCrf.Application.Commands;

public class UpdateCashFlowCommand : CashFlowCommand
{
    public UpdateCashFlowCommand(string cashflowId, string description, double amount, string entry, DateTime date)
    {
        CashFlowId = cashflowId;
        Description = description;
        Amount = amount;
        Entry = entry;
        Date = date;
    }

    public UpdateCashFlowCommand(CashFlow cash)
    {
        CashFlowId = cash.CashFlowId;
        Description = cash.Description;
        Amount = cash.Amount;
        Entry = cash.Entry;
        Date = cash.Date;
        Action = UserAction.Insert;
    }

    public override bool IsValid()
    {
        ValidationResult = new UpdateCashFlowCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
