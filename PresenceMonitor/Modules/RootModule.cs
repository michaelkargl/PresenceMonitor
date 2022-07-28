using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules;

public static class RootModule
{
    public static void Configure(IServiceCollection serviceCollection, IConfiguration contextConfiguration)
    {
        ConfigurationModule.Configure(serviceCollection, contextConfiguration);
        ApplicationModule.Configure(serviceCollection);
    }
}