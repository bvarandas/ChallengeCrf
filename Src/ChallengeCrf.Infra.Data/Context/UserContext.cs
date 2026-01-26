using ChallengeCrf.Domain.Models;
using MongoDB.Driver;
using MongoFramework;

namespace ChallengeCrf.Infra.Data.Context;

public class UserContext : MongoDbContext
{
    public MongoDbSet<User> Users { get; set; } = null!;
    public MongoClient MongoClient { get; }
    public UserContext(IMongoDbConnection connection) : base(connection)
    {
        MongoClient = (MongoClient)connection.GetDatabase().Client;
    }
}
