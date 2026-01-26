using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ChallengeCrf.Api.Controllers;

public static class CashFlowMap
{
    public static void ExposeMaps(WebApplication app)
    {
        app.MapPost("api/cashflow/", async (CashFlow cash, IMediatorHandler _mediator, ILogger<Program> logger) =>
        {
            try
            {
                cash.CashFlowIdTemp = ObjectId.GenerateNewId().ToString();
                var addCommand = new InsertCashFlowCache(cash);
                await _mediator.SendCommand(addCommand);

                return Results.Accepted(null, cash);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });

        app.MapPut("api/cashflow/{id}", async ([FromRoute] string id, CashFlow cash, IMapper _mapper, IMediatorHandler _mediator, ILogger<Program> logger) =>
        {
            try
            {
                var addCommand = new UpdateCashFlowCache(cash);
                await _mediator.SendCommand(addCommand);

                return Results.Accepted(null, cash);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });

        app.MapDelete("api/cashflow/{id}", async (string id, IMapper _mapper, IMediatorHandler _mediator, ILogger<Program> logger) =>
        {
            try
            {
                var addCommand = new RemoveCashFlowCache(id);
                await _mediator.SendCommand(addCommand);

                return Results.Ok(null);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });

        app.MapGet("api/cashflow/", async ([FromServices] ICashFlowService service, ILogger<Program> logger) =>
        {
            try
            {
                var cashList = await service.GetListAllAsync();

                return Results.Ok(cashList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });

        app.MapGet("api/cashflow/{id}", async ([FromServices] ICashFlowService service, string id, ILogger<Program> logger) =>
        {
            try
            {
                var cash = await service.GetCashFlowyIDAsync(id);

                return Results.Ok(cash);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });



        app.MapGet("api/dailyconsolidated", async ([FromQuery] string date, [FromServices] IDailyConsolidatedService service, ILogger<Program> logger) =>
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime dateFilter))
                {
                    return Results.BadRequest("Data inválida");
                }

                var list = await service.GetDailyConsolidatedByDateAsync(dateFilter);

                if (list.IsSuccess && list.Value is not null)
                {
                    return Results.Ok(list.Value);
                }

                return Results.Ok(new DailyConsolidated());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }
        });
    }
}
