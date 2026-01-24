using ChallengeCrf.Application.Validations;

namespace ChallengeCrf.Application.Commands;

public class RemoveCashFlowCache : CashFlowCommand
{
    public RemoveCashFlowCache(string cashflowId)
    {
        CashFlowId = cashflowId;
    }
    public override bool IsValid()
    {
        //ValidationResult = new RemoveCashFlowCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}


public class RemoveCashFlowCommand : CashFlowCommand
{
    public RemoveCashFlowCommand(string cashflowId)
    {
        CashFlowId = cashflowId;
    }
    public override bool IsValid()
    {
        ValidationResult = new RemoveCashFlowCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}