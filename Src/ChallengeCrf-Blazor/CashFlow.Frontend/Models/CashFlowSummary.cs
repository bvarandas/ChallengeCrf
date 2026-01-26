using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashFlow.Frontend.Models;

public class CashFlowSummary
{
    public string cashFlowId { get; set;}
    public string cashFlowIdTemp { get; set;}
    public string description { get; set;}
    public decimal amount { get; set;}
    public string entry { get; set;}
    public DateTime date { get; set;}
}
