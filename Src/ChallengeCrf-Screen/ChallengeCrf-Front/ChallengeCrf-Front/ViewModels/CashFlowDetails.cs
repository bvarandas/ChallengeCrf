using System.ComponentModel.DataAnnotations;

namespace ChallengeCrf_Front.ViewModels;

public class CashFlowDetails
{
    public string? CashFlowId { get; set; }

    [Required]
    [StringLength(50)]
    public required string Description { get; set; }

    [Range(1, 99999999)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "The Entry field is required.")]
    public string Entry { get; set; } = string.Empty;

    public DateTime Date { get; set; }
}
