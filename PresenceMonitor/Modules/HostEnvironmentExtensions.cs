using Microsoft.Extensions.Hosting;

namespace Modules;

internal static class HostEnvironmentExtensions
{
    public static bool IsOffline(this IHostEnvironment hostEnvironment)
        => hostEnvironment.IsEnvironment("Offline")
           || hostEnvironment.IsEnvironment("offline")
           || hostEnvironment.IsEnvironment("OFFLINE");
}