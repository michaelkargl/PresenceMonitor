using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PresenceMonitor.Modules;

public static class RootModule
{
    public static void Configure(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ConfigurationModule.Configure(serviceCollection, configuration);
        MessagingModule.Configure(serviceCollection, configuration);
        UseCasesModule.Configure(serviceCollection);
    }
}