using AutoMapper;
using ChallengeCrf.Api.Configurations;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.CrossCutting.Bus;
using Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Host.UseSerilog(Logging.ConfigureLogger);

NativeInjectorBoostrapper.RegisterServices(builder.Services, config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                 .ExecuteAsync(statusCodeContext.HttpContext));

//app.MapHealthChecks("/healthz");

app.MapPost("api/cashflow/", async (CashFlow cash,
    IMediatorHandler _mediator, ILogger<Program> logger) =>
{
    try
    {
        var addCommand = new InsertCashFlowCommand(cash);
        await _mediator.SendCommand(addCommand);

        return Results.Accepted(null, cash);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});
app.MapPut("api/cashflow/", async (CashFlow cash, IMapper _mapper, IMediatorHandler _mediator, ILogger<Program> logger) =>
{
    try
    {
        var addCommand = new UpdateCashFlowCommand(cash);
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
        var addCommand = new RemoveCashFlowCommand(id);
        await _mediator.SendCommand(addCommand);

        return Results.Ok(null);

    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapGet("api/cashflow/", async (IMediatorHandler _mediator, ILogger<Program> logger) =>
{
    try
    {



        return Results.Ok(null);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapGet("api/cashflow/{id}", async (IMediatorHandler _mediator, string id, ILogger<Program> logger) =>
{
    try
    {

        return Results.Ok(null);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});



app.MapGet("api/dailyconsolidated", async ([FromQuery] string date, ILogger<Program> logger) =>
{
    try
    {
        if (!DateTime.TryParse(date, out DateTime dateFilter))
        {
            return Results.BadRequest("Data inválida");
        }

        return Results.Ok(null);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.UseCors("CorsPolicy");
app.MapHub<BrokerHub>("/hubs/brokerhub");
app.MapControllers();
app.Run();
