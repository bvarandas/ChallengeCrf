using FluentValidation.Results;
namespace ChallengeCrf.Domain.Bus;

public abstract  class Command : Message
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; } = null!;

    protected Command() 
    { 
        Timestamp = DateTime.Now;
    }

    public abstract bool IsValid();
}
