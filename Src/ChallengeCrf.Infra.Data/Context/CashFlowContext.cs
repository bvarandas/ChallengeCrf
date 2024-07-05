using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using MongoDB.Driver;
using MongoFramework;
namespace ChallengeCrf.Infra.Data.Context;
public class CashFlowContext : MongoDbContext
{
    public MongoDbSet<CashFlow> CashFlow { get; set; } = null!;
    public MongoDbSet<DailyConsolidated> DailyConsolidated { get; set; } = null!;
    public CashFlowContext(IMongoDbConnection connection) : base(connection)
    {
    }
}