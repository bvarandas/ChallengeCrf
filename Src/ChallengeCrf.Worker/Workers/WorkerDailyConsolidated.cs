using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Interfaces;
namespace ChallengeCrf.Queue.Worker.Workers;

public class WorkerDailyConsolidated : BackgroundService
{
    private readonly IDailyConsolidatedService _dailyConsolidatedService;
    private readonly ILogger<WorkerDailyConsolidated> _logger;

    public WorkerDailyConsolidated(
        ILogger<WorkerDailyConsolidated> logger, 
        IDailyConsolidatedService dailyConsolidatedService)
    {
        _dailyConsolidatedService = dailyConsolidatedService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _dailyConsolidatedService.GenerateReportDailyConsolidated(stoppingToken);
            await Task.Delay( 60000 * 5, stoppingToken);
        }
    }
}
