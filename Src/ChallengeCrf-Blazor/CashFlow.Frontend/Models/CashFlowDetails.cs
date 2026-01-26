using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace CashFlow.Frontend.Models;

public class CashFlowDetails
{
    // public int Id { get; set; }
    
    // [Required]
    // [StringLength(50)]
    // public required string  Name { get; set; } 
    
    // [Required(ErrorMessage ="The Genre field is required.")]
    // [JsonConverter(typeof(StringConverter))]
    // public string? GenreId { get; set; }
    
    // [Range(1,100)]
    // public decimal Price { get; set; }
    // public DateOnly ReleaseDate { get; set; }

    public string? CashFlowId { get; set;}
    
    [Required]
    [StringLength(50)]
    public required string Description { get; set;}
    
    [Range(1,99999999)]
    public decimal Amount { get; set;}

    [Required(ErrorMessage ="The Entry field is required.")]
    public string Entry { get; set;} = string.Empty;

    public DateTime Date { get; set;}
}
