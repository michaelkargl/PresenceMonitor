using PresenceMonitor.Messaging.Abstractions.Configuration;

namespace PresenceMonitor.Modules;

public static class ConfigurationModule
{
    public static void Configure(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        void AddOption<T>() where T : class => serviceCollection
            .AddOptions<T>()
            .Bind(configuration.GetSection(typeof(T).Name));

        AddOption<MqttOptions>();
    }
}