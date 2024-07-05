using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Validations;

namespace ChallengeCrf.Domain.Validations
{
    public class UpdateCashFlowCommandValidation : CashFlowValidation<UpdateCashFlowCommand>
    {
        public UpdateCashFlowCommandValidation() 
        {
            ValidateCashFlowId();
            ValidateDescription();
            ValidateCashValue();
            ValidateCashDirection();
            ValidateDate();
        }    
    }
}