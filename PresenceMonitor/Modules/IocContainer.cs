using Configuration;
using Dapr.Client;
using Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Modules;

public static class IocContainer
{
    public static void Configure(
        IServiceCollection serviceCollection,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration
    ) => serviceCollection
        .AddDaprClient()
        .AddDaprMessaging<RawDaprPublisherOptions>(configuration)
        .AddDaprMessaging<PresenceMessageV1DaprPublisherOptions>(configuration)
        .AddMonitorPresenceWorker();

    // https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-client/dotnet-daprclient-usage/#lifetime-management
    // DaprClient is thresd-safe and designed to be a singleton
    private static IServiceCollection AddDaprClient(
        this IServiceCollection collection
    ) => collection
        .AddOption<DaprClientOptions>()
        .AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<DaprClientOptions>>();
            return new DaprClientBuilder()
                .UseGrpcEndpoint(options.Value.DaprEndpoint)
                .Build();
        });

    private static IServiceCollection AddDaprMessaging<TPublisherOptions>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) where TPublisherOptions : AbstractDaprPublisherOptions
    {
        // TODO: register FakePublisher if disabled by config
        return serviceCollection
            .AddOption<TPublisherOptions>()
            .AddTransient(typeof(IMessagePublisher<,>), typeof(DaprMessagePublisher<,>));
    }

    private static IServiceCollection AddMonitorPresenceWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOption<MonitorPresenceWorkerOptions>();
        serviceCollection.AddHostedService<MonitorPresenceWorker>();
        return serviceCollection;
    }
}