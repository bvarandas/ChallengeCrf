using ChallengeCrf.Application.Commands;

namespace ChallengeCrf.Application.Validations
{
    public class RemoveCashFlowCommandValidation : CashFlowValidation<RemoveCashFlowCommand>
    {
        public RemoveCashFlowCommandValidation()
        {
            ValidateCashFlowId();
        }
    }
}
