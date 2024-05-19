using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using PresenceMonitor.Utilities.Ioc;

public interface IDaprClientProvider : ISingletonProvider<DaprClient>
{
}

public sealed class DaprClientProvider : SingletonProvider<DaprClient>, IDaprClientProvider
{
    public DaprClientProvider(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {
    }
}