using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ChallengeCrf.Application.Services;

public class OutboxProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOutboxCache _cache;
    private readonly ILogger<OutboxProcessorService> _logger;
    public OutboxProcessorService(IServiceScopeFactory scopeFactory, IOutboxCache cache, ILogger<OutboxProcessorService> logger)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var messageBroker = scope.ServiceProvider.GetRequiredService<IQueueProducer>();

                var listCashFlow = await _cache.GetCashFlowOutboxAsync();

                var pendingMessages = listCashFlow.Value;

                foreach (var message in pendingMessages)
                {
                    try
                    {
                        var cash = JsonSerializer.Deserialize<CashFlow>(message.Content);

                        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);

                        envelopeMessage.LastTransaction = DateTime.Now;

                        envelopeMessage.Action = message.UserAction;

                        await messageBroker.PublishMessageAsync(envelopeMessage);

                        await _cache.RemoveCashflowAsync(cash);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        message.Error = ex.Message;
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Poll every 5 seconds
        }
    }
}
