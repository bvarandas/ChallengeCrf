using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace ChallengeCrf.Domain.Models;


[ProtoContract]
public sealed class CashFlow 
{
    //[BsonId]
    //[ProtoMember(8)]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [ProtoMember(8)]
    public string cashFlowIdTemp = string.Empty;

    [ProtoMember(1)]
    public string CashFlowId 
    { 
        get 
        {
            //cashFlowIdTemp = Id.ToString();
            return cashFlowIdTemp; 
        } 
        set
        {
            cashFlowIdTemp = Id.ToString(); 
        }
    }

    [ProtoMember(2)]
    public string Description { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public double Amount { get; set; }

    [ProtoMember(4)]
    public string Entry { get; set; } = string.Empty;

    [ProtoMember(5)]
    public DateTime Date { get; set; }


    [ProtoMember(7)]
    public DailyConsolidated DailyConsolidated { get; set; } = null!;
    public CashFlow(string id, string cashFlowId, string description, double cashValue, string entry, DateTime date)
    {
        Id = new ObjectId(id);
        CashFlowId = cashFlowId;
        Description = description;
        Amount = cashValue;
        Entry = entry;
        Date = date;
    }

    public CashFlow(string description, double cashValue, string entry, DateTime date)
    {
        Description = description;
        Amount = cashValue;
        Entry = entry;
        Date = date;
    }

    public CashFlow() { }
}