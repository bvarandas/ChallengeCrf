using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using ChallengeCrf.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ChallengeCrf.Application.Services;

public class OutboxProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOutboxCache _cache;
    public OutboxProcessorService(IServiceScopeFactory scopeFactory, IOutboxCache cache)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OutboxContext>();
                var messageBroker = scope.ServiceProvider.GetRequiredService<IQueueProducer>();

                var listCashFlow = await _cache.GetCashFlowOutboxAsync();

                var pendingMessages = listCashFlow.Value;

                foreach (var message in pendingMessages)
                {
                    try
                    {
                        var cash = JsonSerializer.Deserialize<CashFlow>(message.Content);

                        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
                        envelopeMessage.Action = UserAction.Insert;
                        envelopeMessage.LastTransaction = DateTime.Now;
                        cash.Date = DateTime.Now;

                        await messageBroker.PublishMessageAsync(envelopeMessage);

                        // Mark as processed
                        message.ProcessedOnUtc = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Log the error and potentially set the Error field
                        message.Error = ex.Message;
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Poll every 5 seconds
        }
    }
}
