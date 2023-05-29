using Microsoft.Extensions.DependencyInjection;

namespace Modules;

public static class UseCasesModule
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(typeof(ProcessMessageCommandHandler).Assembly)
        );
    }
}