using Configuration;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modules;

public static class ApiModule
{
    public static void Configure(IServiceCollection serviceCollection, IHostEnvironment environment)
    {
        serviceCollection.AddPresenceApi(environment);
    }

    private static IServiceCollection AddPresenceApi(
        this IServiceCollection serviceCollection, IHostEnvironment hostEnvironment
    )
    {
        serviceCollection
            .AddOption<PresenceApiOptions>()
            .AddHttpClient<IPresenceService, PresenceService>();
        
        if (hostEnvironment.IsEnvironment("Offline"))
        {
            serviceCollection.AddTransient<IPresenceService, RandomPresenceService>();
        }

        return serviceCollection;
    }
}