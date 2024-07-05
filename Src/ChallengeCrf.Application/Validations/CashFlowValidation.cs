using ChallengeCrf.Application.Commands;
using ChallengeCrf.Domain.Constants;
using FluentValidation;

namespace ChallengeCrf.Application.Validations;

public abstract class CashFlowValidation<T> : AbstractValidator<T> where T : CashFlowCommand
{
    protected void ValidateCashFlowId()
    {
        RuleFor(c => c.CashFlowId)
            .NotEqual("");
    }

    protected void ValidateDescription()
    {
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("É necessário inserir a Descrição!")
            .Length(3, 150).WithMessage("É necessário inserir ao menos 3 caracteres na Descrição!");
    }

    protected void ValidateCashValue()
    {
        RuleFor(c => c.Amount)
            .NotEqual(0)
            .WithMessage("É necessário inserir um valor válido!")
            .GreaterThan(0)
            .WithMessage("É necessário inserir um valor positivo!");
    }

    protected void ValidateCashDirection()
    {
        RuleFor(c => c.Entry.ToLower())
            .NotEmpty().WithMessage("É necessário inserir o Lançamento!")
            .Must(IsValidEntry)
            .WithMessage("É necessário inserir Débito ou Crédito válido!");
    }

    protected void ValidateDate()
    {
        RuleFor(c => c.Date)
            .NotEmpty()
            .WithMessage("É necessário inserir a Data !")
            .Must(d=>IsValidDate(d.ToString()))
            .WithMessage("É necessário inserir uma data válida!");
    }

    private bool IsValidEntry(string value)
    => value == CashFlowEntry.Credit || value == CashFlowEntry.Debit;

    private bool IsValidDate(string value)
    {
        DateTime date;
        return DateTime.TryParse(value, out date);
    }
}
