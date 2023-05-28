using Configuration;
using Dapr.Client;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            .AddTransient(CreatePresenceService);


        if (hostEnvironment.IsOffline())
        {
            serviceCollection.AddTransient<IPresenceService, RandomPresenceService>();
        }

        return serviceCollection;
    }

    private static IPresenceService CreatePresenceService(IServiceProvider services)
    {
        var apiOptions = services.GetRequiredService<IOptions<PresenceApiOptions>>();
        var invokeHttpClient = DaprClient.CreateInvokeHttpClient(
            apiOptions.Value.AppId,
            apiOptions.Value.DaprEndpoint);
        return new PresenceService(invokeHttpClient, apiOptions);
    }
}