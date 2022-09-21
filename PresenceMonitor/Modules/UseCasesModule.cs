using MediatR;
using PresenceMonitor.UseCases;

namespace PresenceMonitor.Modules;

public static class UseCasesModule
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(typeof(ProcessMessageCommandHandler).Assembly);
    }
}