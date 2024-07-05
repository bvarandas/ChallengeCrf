using ChallengeCrf.Application.Commands;
namespace ChallengeCrf.Application.Validations;

public class InsertCashFlowCommandValidation : CashFlowValidation<InsertCashFlowCommand>
{
    public InsertCashFlowCommandValidation()
    {
        ValidateDescription();
        ValidateCashDirection();
        ValidateCashValue();
        ValidateDate();
    }
}
