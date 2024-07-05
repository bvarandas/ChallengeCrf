using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeCrf.Domain.Models;

[ProtoContract]
public sealed class DailyConsolidated
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    public string DailyConsolidatedId { get; set; }

    [ProtoMember(2)]
    public double AmountCredit { get; set; }

    [ProtoMember(3)]
    public double AmountDebit { get; set; }

    [ProtoMember(4)]
    public DateTime Date { get; set; }

    [ProtoMember(5)]
    public double AmountTotal { get; set; }

    [ProtoMember(6)]
    public IEnumerable<CashFlow> CashFlows { get; set; }

    public DailyConsolidated()
    {
    }

    public DailyConsolidated(string dailyConsolidatedId, double amountCredit, double amountDebit,double amountTotal,  DateTime date, IEnumerable<CashFlow> cashFlows)
    {
        DailyConsolidatedId = dailyConsolidatedId;
        AmountCredit = amountCredit;
        AmountDebit = amountDebit;
        Date = date;
        AmountTotal = amountTotal;
        CashFlows = cashFlows;
    }
}
