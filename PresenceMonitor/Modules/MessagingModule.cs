using MQTTnet;
using MQTTnet.Client;
using PresenceMonitor.Messaging.Abstractions;
using PresenceMonitor.Messaging.Mqtt;
using PresenceMonitor.Worker;

namespace PresenceMonitor.Modules;

public static class MessagingModule
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<ProcessMessageWorker>();
        serviceCollection.AddSingleton(new Lazy<IMqttClient>(
            () => new MqttFactory().CreateMqttClient()
        ));
        serviceCollection.AddSingleton<IMessageSubscriber, MqttMessageSubscriber>();
    }
}