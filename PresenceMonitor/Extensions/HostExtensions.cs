using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PresenceMonitor.Extensions;

public static class HostExtensions
{
    public static async Task HostAsync(this IHost host)
    {
        var logger = host.Services.GetRequiredService<ILogger<IHost>>();
        
        logger.LogDebug("Starting host...");
        logger.LogDebug("Running application...");
        await host.RunAsync();
        
        logger.LogDebug("Application stopped");
    }
}