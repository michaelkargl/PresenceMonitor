using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules;

namespace Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureIocModules(
        this IHostBuilder hostBuilder
    ) => hostBuilder.ConfigureServices(ConfigureRootModule);
    
    private static void ConfigureRootModule(
        HostBuilderContext context,
        IServiceCollection collection
    ) => RootModule.Configure(collection, context.Configuration);
}