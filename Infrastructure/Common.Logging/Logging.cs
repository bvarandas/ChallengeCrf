using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (context, loggerConfiguration) =>
        {
            var env = context.HostingEnvironment;
            loggerConfiguration.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplpicationName", env.ApplicationName)
            .Enrich.WithProperty("ApplpicationName", env.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console();

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override("ChallengeCrf.Api", LogEventLevel.Debug);
                loggerConfiguration.MinimumLevel.Override("ChallengeCrf.Application", LogEventLevel.Debug);
                loggerConfiguration.MinimumLevel.Override("ChallengeCrf.Queue.Worker", LogEventLevel.Debug);
            }

            var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                    IndexFormat = "CashFlow-Logs-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Information 
                    | LogEventLevel.Debug 
                    | LogEventLevel.Error,
                });
            }
        };
}