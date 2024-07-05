using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Queue.Worker.Workers;

public class WorkerMessage : BackgroundService
{
    private readonly ICashFlowService _flowService;
    private readonly IDailyConsolidatedService _dailyConsolidatedService;
    private Thread ThreadMessageSender = null!;
    public WorkerMessage(ICashFlowService registerService,
        IDailyConsolidatedService dailyConsolidatedService)
    {
        _dailyConsolidatedService = dailyConsolidatedService;
        _flowService = registerService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        return Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken stoppingToken)
    {
        ThreadMessageSender = new Thread(() => MessageSender(stoppingToken));
        ThreadMessageSender.Name = nameof(ThreadMessageSender);
        ThreadMessageSender.Start();

        return Task.CompletedTask;
    }
    private async Task MessageSender(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            await ConsumerDailyConsolidatedSender();
            await CashFlowSender();

            Thread.Sleep(5000);
        }
    }
    private async Task ConsumerDailyConsolidatedSender()
    {
        var dailyConsolidated = await _dailyConsolidatedService.GetDailyConsolidatedByDateAsync(DateTime.Now);
        var list = new List<DailyConsolidated>();

        if (dailyConsolidated.IsSuccess)
            await WorkerProducer._Singleton.PublishMessages(dailyConsolidated.Value);
    }

    private async Task CashFlowSender()
    {
        var registerlist = await _flowService.GetListAllAsync();

        if (registerlist is not null)
        {
            await WorkerProducer._Singleton.PublishMessages(registerlist.ToListAsync().Result);
        }
        
    }
}
