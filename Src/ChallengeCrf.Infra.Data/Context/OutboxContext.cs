using ChallengeCrf.Domain.Models;
using MongoDB.Driver;
using MongoFramework;
namespace ChallengeCrf.Infra.Data.Context;
public class OutboxContext : MongoDbContext
{
    public MongoDbSet<CashFlow> CashFlow { get; set; } = null!;
    public MongoDbSet<OutboxMessage> OutboxDbSet { get; set; } = null!;
    public MongoClient MongoClient { get; }
    public OutboxContext(IMongoDbConnection connection) : base(connection)
    {
        MongoClient = (MongoClient)connection.GetDatabase().Client;
    }
}
