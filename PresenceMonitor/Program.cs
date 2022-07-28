using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureIocModules()
    .Build();

await host.HostAsync(provider =>
{
    var application = provider.GetRequiredService<Application>();
    return application.RunAsync(args);
});