using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            .AddHttpClient();
        
        ApiModule.Configure(serviceCollection, configuration);
        MessagingModule.Configure(serviceCollection, environment, configuration);
        UseCasesModule.Configure(serviceCollection);
    }
}