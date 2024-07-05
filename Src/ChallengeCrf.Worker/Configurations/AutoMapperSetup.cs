using AutoMapper;
using ChallengeCrf.Application.AutoMapper;

namespace ChallengeCrf.Queue.Worker.Configurations;

public static class AutoMapperSetup
{
    public static void AddAutoMapperSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentException(nameof(services));

        var mapper = AutoMapperConfig
            .RegisterMappings()
            .CreateMapper();

        

        services.AddSingleton(mapper);
    }
}
