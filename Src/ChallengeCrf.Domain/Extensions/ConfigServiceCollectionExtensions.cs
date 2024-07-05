using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Domain.Extesions;

public static class ConfigServiceCollectionExtensions
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<QueueCommandSettings>(config.GetSection(nameof(QueueCommandSettings)));
        services.Configure<QueueEventSettings>(config.GetSection(nameof(QueueEventSettings)));

        return services;
    }
}
