using Microsoft.Extensions.Hosting;

namespace Extensions;

public static class HostExtensions
{
    /// <summary>
    /// Host the specified <paramref name="hostFunctionAsync"/>. 
    /// </summary>
    /// <param name="host">The target host to run the function in.</param>
    /// <param name="hostFunctionAsync">An asynchronous function to host. In most cases, this runs the main execution loop.</param>
    public static async Task HostAsync(
        this IHost host,
        Func<IServiceProvider, Task> hostFunctionAsync
    )
    {
        try
        {
            await host.StartAsync();
            await hostFunctionAsync(host.Services);
        }
        finally
        {
            await host.StopAsync();
        }
    }
}