using ChallengeCrf.Application.Commands;
using ChallengeCrf.Domain.Models;
using FluentResults;
namespace ChallengeCrf.Appplication.Interfaces;
public interface IDailyConsolidatedService
{
    Task<Result<DailyConsolidated>> GetDailyConsolidatedByDateAsync(DateTime date);
    Task<InsertDailyConsolidatedCommand> InsertDailyConsolidatedAsync(DailyConsolidated dailyConsolidated);
    Task GenerateReportDailyConsolidated(CancellationToken stoppingToken);
}