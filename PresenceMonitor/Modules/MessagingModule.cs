using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PresenceMonitor.Messaging.Abstractions;
using PresenceMonitor.Messaging.Mqtt;
using PresenceMonitor.Worker;

namespace PresenceMonitor.Modules;

public static class MessagingModule
{
    public static void Configure(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddHostedService<ProcessMessageWorker>();
        serviceCollection.AddSingleton<IMessageSubscriber, MqttMessageSubscriber>();
    }
}