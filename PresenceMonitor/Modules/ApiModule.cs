using Configuration;
using Dapr.Client;
using Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Modules;

public static class ApiModule
{
    public static void Configure(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddPresenceApi(configuration);
    }

    private static IServiceCollection AddPresenceApi(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection
            .AddOption<PresenceApiOptions>()
            .AddTransient<RandomPresenceService>()
            .AddTransient<PresenceService>(CreatePresenceService);

        var apiOptions = configuration.GetRequiredOption<PresenceApiOptions>();
        serviceCollection.AddTransient<IPresenceService, PresenceService, RandomPresenceService>(
            apiOptions.Enabled,
            CreatePresenceService
        );

        return serviceCollection;
    }

    private static PresenceService CreatePresenceService(IServiceProvider services)
    {
        var apiOptions = services.GetRequiredService<IOptions<PresenceApiOptions>>();
        var invokeHttpClient = DaprClient.CreateInvokeHttpClient(
            apiOptions.Value.AppId,
            apiOptions.Value.DaprEndpoint);
        return new PresenceService(invokeHttpClient, apiOptions);
    }
}