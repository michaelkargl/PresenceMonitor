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
}