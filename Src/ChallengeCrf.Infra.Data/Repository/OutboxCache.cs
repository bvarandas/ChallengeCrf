using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;


namespace ChallengeCrf.Infra.Data.Repository
{
    public class OutboxCache : IOutboxCache
    {
        private readonly ILogger<OutboxCache> _logger;
        private readonly ConnectionDragonflyDB _config;
        private readonly ConnectionMultiplexer _dragonfly;
        private readonly IDatabase _db;

        private RedisKey keycash = new RedisKey("cashflow");
        private RedisKey keyoutbox = new RedisKey("outbox");

        public OutboxCache(IOptions<ConnectionDragonflyDB> config, ILogger<OutboxCache> logger)
        {
            _logger = logger;
            _config = config.Value;

            _dragonfly = ConnectionMultiplexer.Connect(_config.ConnectionString, options =>
            {
                options.ReconnectRetryPolicy = new ExponentialRetry(5000, 1000 * 60);
            });

            _db = _dragonfly.GetDatabase(0);
        }

        public async Task<Result<IEnumerable<OutboxMessage>>> GetCashFlowOutboxAsync()
        {
            var hash = new List<OutboxMessage>();

            var result = Enumerable.Empty<OutboxMessage>();

            var hashEntry = await _db.HashGetAllAsync(keyoutbox);

            foreach (var item in hashEntry)
            {
                var value = JsonSerializer.Deserialize<OutboxMessage>(item.Value!);
                hash.Add(value!);
            }

            result = hash;

            return Result.Ok(result);
        }

        public async Task<Result> UpsertCashflowAsync(OutboxMessage outboxMessage, string cashFlowIdTemp)
        {
            try
            {
                IDatabase db = _dragonfly.GetDatabase();
                ITransaction transaction = db.CreateTransaction();

                RedisValue value = new RedisValue(outboxMessage.Content);
                RedisValue valueOutbox = new RedisValue(JsonSerializer.Serialize(outboxMessage));

                _ = transaction.HashSetAsync(keycash, new HashEntry[] { new HashEntry(new RedisValue(cashFlowIdTemp), value) });

                _ = transaction.HashSetAsync(keyoutbox, new HashEntry[] { new HashEntry(new RedisValue(cashFlowIdTemp), valueOutbox) });

                bool committed = await transaction.ExecuteAsync();

                _logger.LogInformation(committed ? "Cashflow outbox inserido com sucesso" : "Problema ao inserir cashflow");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Result.Fail(new Error(ex.Message));
            }

            return Result.Ok();
        }

        public async Task<Result> RemoveCashflowAsync(CashFlow cash)
        {
            ITransaction transaction = _db.CreateTransaction();

            try
            {
                RedisValue value = new RedisValue(cash.CashFlowIdTemp);

                _ = transaction.HashDeleteAsync(keycash, value);

                _ = transaction.HashDeleteAsync(keyoutbox, value);

                bool committed = await transaction.ExecuteAsync();

                _logger.LogInformation(committed ? "Cashflow outbox removido com sucesso" : "Problema ao remover cashflow");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Result.Fail(new Error(ex.Message));
            }

            return Result.Ok();
        }
    }
}
