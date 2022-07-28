using Microsoft.Extensions.DependencyInjection;

namespace Modules;

public static class ApplicationModule
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Application>();
    }   
}