using ChallengeCrf.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MongoFramework;

namespace ChallengeCrf.Infra.Data.Context;

public class EventStoreSqlContext : MongoDbContext
{
    public DbSet<StoredEvent> StoredEvent { get; set; } = null!;
    private readonly IHostEnvironment _env;

    public EventStoreSqlContext(IMongoDbConnection connection, IHostEnvironment env) : base(connection)
    {
        _env = env;
    }
}