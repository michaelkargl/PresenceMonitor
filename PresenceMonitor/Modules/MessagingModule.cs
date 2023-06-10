using Configuration;
using Dapr.Client;
using Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
        return serviceCollection
            .AddOption<DaprPublisherOptions>()
            .AddTransient<IMessagePublisher, DaprMessagePublisher, FakeMessagePublisher>(
                daprPublisherOptions!.Enabled,
                BuildDaprMessagePublisher
            );
    }

    private static DaprMessagePublisher BuildDaprMessagePublisher(IServiceProvider serviceProvider)
    {
        var daprPublisherOptions = serviceProvider.GetRequiredService<IOptions<DaprPublisherOptions>>();
        var daprClient = new DaprClientBuilder()
            .UseGrpcEndpoint(daprPublisherOptions.Value.DaprEndpoint)
            .Build();
        return new DaprMessagePublisher(daprClient, daprPublisherOptions);
    }

    private static IServiceCollection AddMonitorPresenceWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOption<MonitorPresenceWorkerOptions>();
        serviceCollection.AddHostedService<MonitorPresenceWorker>();
        return serviceCollection;
    }
}