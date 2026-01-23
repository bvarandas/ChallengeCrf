using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
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
                var messageBroker = scope.ServiceProvider.GetRequiredService<IQueueProducer>();

                var listCashFlow = await _cache.GetCashFlowOutboxAsync();

                var pendingMessages = listCashFlow.Value;

                foreach (var message in pendingMessages)
                {
                    try
                    {

                        var envelopeMessage = JsonSerializer.Deserialize<EnvelopeMessage<CashFlow>>(message.Content);
                        envelopeMessage.LastTransaction = DateTime.Now;

                        await messageBroker.PublishMessageAsync(envelopeMessage);

                        var cash = envelopeMessage.Body;

                        await _cache.RemoveCashflowAsync(cash);
                    }
                    catch (Exception ex)
                    {
                        // Log the error and potentially set the Error field
                        message.Error = ex.Message;

                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Poll every 5 seconds
        }
    }
}
