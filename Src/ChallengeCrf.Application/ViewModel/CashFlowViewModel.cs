using ProtoBuf;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChallengeCrf.Application.ViewModel;

[ProtoContract]
public class CashFlowViewModel
{
    [Key]
    [ProtoMember(1)]
    public string CashFlowId { get; set; }=string.Empty;

    [Required(ErrorMessage = "A Descrição é necessária")]
    [MinLength(2)]
    [MaxLength(100)]
    [DisplayName("Description")]
    [ProtoMember(2)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "O valor é necessário")]
    [MinLength(1)]
    [MaxLength(1)]
    [DisplayName("Amount")]
    [ProtoMember(3)]
    public double Amount { get; set; }


    [Required(ErrorMessage = "O Lançamento (Debito ou credito) é necessário")]
    [MinLength(1)]
    [MaxLength(100)]
    [DisplayName("Entry")]
    [ProtoMember(5)]
    public string Entry { get; set; } = string.Empty;

    //[Required(ErrorMessage = "A Data é necessária")]
    [MinLength(1)]
    [MaxLength(100)]
    [DisplayName("Date")]
    [ProtoMember(6)]
    public DateTime Date { get; set; }

    [MinLength(1)]
    [MaxLength(10)]
    [DisplayName("UserAction")]
    [ProtoMember(7)]
    public string UserAction { get; set; } = string.Empty;

    public CashFlowViewModel(string cashFlowId, string description, double amount, string entry, DateTime date, string userAction)
    {
        CashFlowId = cashFlowId;
        Description = description;
        Amount = amount;
        Entry = entry;
        Date = date;
        UserAction = userAction;
    }

    public CashFlowViewModel()
    { }
}
