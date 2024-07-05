using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.ComponentModel;
using System.Text.Json;

namespace ChallengeCrf.Infra.CrossCutting.Middleware;

public class ConfigurationGlobalException : IMiddleware
{
    private readonly ILogger<ConfigurationGlobalException> _logger = null!;

    public ConfigurationGlobalException(ILogger<ConfigurationGlobalException> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (MongoException ex)
        {
            _logger.LogError($"Erro inesperado: {ex}");
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            if (!(ex is Win32Exception w32ex))
                w32ex = ex.InnerException as Win32Exception;

            int statusCode = w32ex != null ? w32ex.ErrorCode : ex.HResult;

            _logger.LogError($"Erro inesperado: {ex}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var json = JsonSerializer.Serialize(new 
        { 
            statusCode, 
            message = "Ocorreu um erro no processamento da requisição" 
        });

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(json);
    }
}
