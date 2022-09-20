using PresenceMonitor.Modules;

namespace PresenceMonitor.Extensions;

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