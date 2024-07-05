using ChallengeCrf.Api.Hubs;
using ChallengeCrf.Api.Configurations;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Api.Producer;
using Serilog;
using ProtoBuf.Meta;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Common.Logging;
using ChallengeCrf.Application.Interfaces;
using Common.Logging.Correlation;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.ValueObjects;
using System.Security.Policy;

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

app.MapPost("api/cashflow/",  async (CashFlow cash, IQueueProducer queueProducer, ILogger<Program> logger ) => 
{
    try
    {
        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
        envelopeMessage.Action = UserAction.Insert;
        envelopeMessage.LastTransaction = DateTime.Now;
        cash.Date = DateTime.Now;
        
        await queueProducer.PublishMessageAsync(envelopeMessage);

        return Results.Accepted(null,cash);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});
app.MapPut("api/cashflow/", async (CashFlowViewModel cashModel, IQueueProducer queueProducer, ILogger<Program> logger) => 
{
    try
    {
        CashFlow cash = new CashFlow(cashModel.CashFlowId, cashModel.CashFlowId, cashModel.Description, cashModel.Amount, cashModel.Entry, DateTime.Now);
        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
        envelopeMessage.Action = UserAction.Update;
        envelopeMessage.LastTransaction = DateTime.Now;

        cash.Id = new MongoDB.Bson.ObjectId(cashModel.CashFlowId);
        cash.cashFlowIdTemp = cashModel.CashFlowId;
        
        await queueProducer.PublishMessageAsync(envelopeMessage);

        return Results.Accepted(null, cash);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapGet("api/cashflow/", async (IQueueProducer queueProducer, ILogger<Program> logger) => {
    try
    {
        var cash = new CashFlow("",0,"",DateTime.Now);
        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
        envelopeMessage.Action = UserAction.GetAll;

        await queueProducer.PublishMessageAsync(envelopeMessage);

        return Results.Ok(null);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapGet("api/cashflow/{id}", async (IQueueProducer queueProducer, string id, ILogger<Program> logger) =>
{
    try
    {
        var cash = new CashFlow("", 0, "", DateTime.Now) 
        { 
            CashFlowId = id, cashFlowIdTemp= id , Id = new MongoDB.Bson.ObjectId(id) 
        };

        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
        envelopeMessage.Action = UserAction.Get;

        await queueProducer.PublishMessageAsync(envelopeMessage);

        return Results.Ok(null);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapDelete("api/cashflow/{id}", async (string id, IQueueProducer queueProducer,  ILogger<Program> logger) => 
{
    try
    {
        var cash = new CashFlow("", 0, "", DateTime.Now)
        {
            CashFlowId = id,
            cashFlowIdTemp = id,
            Id = new MongoDB.Bson.ObjectId(id)
        };
        
        var envelopeMessage = new EnvelopeMessage<CashFlow>(cash);
        envelopeMessage.Action = UserAction.Delete;

        await queueProducer.PublishMessageAsync(envelopeMessage);

        return Results.Ok(null);

    }catch(Exception ex)
    {
        logger.LogError(ex, $"{ex.Message}");
        return Results.BadRequest(ex);
    }
});

app.MapGet("api/dailyconsolidated", async ([FromQuery]string date, IQueueProducer queueProducer, ILogger<Program> logger) => 
{
    try
    {
        if (!DateTime.TryParse(date, out DateTime dateFilter))
        {
            return Results.BadRequest("Data inválida");
        }

        var dailyConsolidated = new DailyConsolidated("", 0, 0,0, dateFilter, null) ;

        var envelopeMessage = new EnvelopeMessage<DailyConsolidated>(dailyConsolidated);
        envelopeMessage.Action = UserAction.Get;

        await queueProducer.PublishMessageAsync(envelopeMessage);


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
