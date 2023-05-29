using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modules;

public static class RootModule
{
    public static void Configure(IServiceCollection serviceCollection, IHostEnvironment environment)
    {
        ApiModule.Configure(serviceCollection, environment);
        MessagingModule.Configure(serviceCollection, environment);
        UseCasesModule.Configure(serviceCollection);
    }
}