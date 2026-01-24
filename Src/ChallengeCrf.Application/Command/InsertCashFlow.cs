using ChallengeCrf.Application.Validations;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Application.Commands;

public class InsertCashFlowCommand : CashFlowCommand
{
    public InsertCashFlowCommand(string description, double amount, string entry, DateTime date)
    {
        Description = description;
        Amount = amount;
        Entry = entry;
        Date = date;

    }

    public InsertCashFlowCommand(CashFlow cash)
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
        ValidationResult = new InsertCashFlowCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class InsertCashFlowCache : CashFlowCommand
{
    public InsertCashFlowCache(string description, double amount, string entry, DateTime date)
    {
        Description = description;
        Amount = amount;
        Entry = entry;
        Date = date;

    }

    public InsertCashFlowCache(CashFlow cash)
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
        //ValidationResult = new InsertCashFlowCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}