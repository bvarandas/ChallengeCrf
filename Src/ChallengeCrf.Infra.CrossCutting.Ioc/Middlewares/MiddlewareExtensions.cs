using ChallengeCrf.Infra.CrossCutting.Middleware;
using Microsoft.AspNetCore.Builder;
namespace ChallengeCrf.Infra.CrossCutting.Ioc.Middlewares;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(
        this IApplicationBuilder app)
        => app.UseMiddleware<ConfigurationGlobalException>();
}