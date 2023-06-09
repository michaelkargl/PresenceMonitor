using Configuration;
using Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modules;

public static class MessagingModule
{
    public static void Configure(
        IServiceCollection serviceCollection,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration
    ) => serviceCollection
        .ConfigureDaprMessaging(configuration.GetOption<DaprPublisherOptions>())
        .AddMonitorPresenceWorker();

    private static IServiceCollection ConfigureDaprMessaging(
        this IServiceCollection serviceCollection,
        DaprPublisherOptions? daprPublisherOptions
    )
    {
        serviceCollection.AddHttpClient<DaprMessagePublisher>();
        return serviceCollection
            .AddOption<DaprPublisherOptions>()
            .AddTransient<IMessagePublisher, DaprMessagePublisher, FakeMessagePublisher>(
                daprPublisherOptions!.Enabled
            );
    }

    private static IServiceCollection AddMonitorPresenceWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOption<MonitorPresenceWorkerOptions>();
        serviceCollection.AddHostedService<MonitorPresenceWorker>();
        return serviceCollection;
    }
}