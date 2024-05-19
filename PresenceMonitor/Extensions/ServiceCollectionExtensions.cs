using Microsoft.Extensions.DependencyInjection;

namespace Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOption<T>(this IServiceCollection serviceCollection) where T : class
    {
        serviceCollection
            .AddOptions<T>()
            .BindConfiguration(typeof(T).Name);
        return serviceCollection;
    }

    public static IServiceCollection AddTransient<TInterface, TServiceEnabled, TServiceFallback>(
        this IServiceCollection serviceCollection,
        bool enabled,
        Func<System.IServiceProvider, TServiceEnabled>? enabledServiceBuilder = null
    ) where TInterface : class
        where TServiceEnabled : class, TInterface
        where TServiceFallback : class, TInterface
    {
        if (!enabled)
        {
            return serviceCollection.AddTransient<TInterface, TServiceFallback>();
        }

        return enabledServiceBuilder is not null
            ? serviceCollection.AddTransient<TInterface, TServiceEnabled>(enabledServiceBuilder)
            : serviceCollection.AddTransient<TInterface, TServiceEnabled>();
    }
}