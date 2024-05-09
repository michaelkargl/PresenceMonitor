using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PresenceMonitor.Utilities.Ioc;

namespace Modules;

public static class RootModule
{
    public static void Configure(
        IServiceCollection serviceCollection,
        IHostEnvironment environment,
        IConfiguration configuration
    )
    {
        serviceCollection
            .AddHttpClient()
            .AddTransient(typeof(ISingletonProvider<>), typeof(SingletonProvider<>));
        
        ApiModule.Configure(serviceCollection, configuration);
        MessagingModule.Configure(serviceCollection, environment, configuration);
        UseCasesModule.Configure(serviceCollection);
    }
}