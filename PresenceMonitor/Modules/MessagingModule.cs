using Configuration;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;

namespace Modules;

public static class MessagingModule
{
    public static void Configure(IServiceCollection serviceCollection, IHostEnvironment hostEnvironment)
    {
        serviceCollection.AddOption<MqttOptions>();
        serviceCollection.AddTransient<Lazy<IMqttClient>>(_ =>
                new Lazy<IMqttClient>(() => new MqttFactory().CreateMqttClient())
            ).AddMqttSubscriber()
            .AddMqttPublisher(hostEnvironment);

        serviceCollection
            .AddMonitorPresenceWorker()
            .AddHostedService<ProcessMessageWorker>();
    }

    private static IServiceCollection AddMonitorPresenceWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOption<MonitorPresenceWorkerOptions>();
        serviceCollection.AddHostedService<MonitorPresenceWorker>();
        return serviceCollection;
    }

    private static IServiceCollection AddMqttSubscriber(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMessageSubscriber, MqttMessageSubscriber>();
        return serviceCollection;
    }

    private static IServiceCollection AddMqttPublisher(
        this IServiceCollection serviceCollection, IHostEnvironment hostEnvironment
    )
    {
        serviceCollection.AddSingleton<IMessagePublisher, MqttMessagePublisher>();
        if (hostEnvironment.IsOffline())
        {
            serviceCollection.AddSingleton<IMessagePublisher, MqttFakeMessagePublisher>();
        }

        return serviceCollection;
    }
}