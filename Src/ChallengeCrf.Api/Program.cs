using ChallengeCrf.Api.Configurations;
using ChallengeCrf.Api.Controllers;
using ChallengeCrf.Infra.CrossCutting.Bus;
using Common.Logging;
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

app.UseResponseCompression();

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

CashFlowMap.ExposeMaps(app);
UserMap.ExposeMaps(app);

app.UseCors("CorsPolicy");
app.MapHub<BrokerHub>("/hubs/brokerhub");
app.MapControllers();
app.Run();
