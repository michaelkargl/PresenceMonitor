using PresenceMonitor.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureIocModules()
    .Build();

await host.HostAsync();