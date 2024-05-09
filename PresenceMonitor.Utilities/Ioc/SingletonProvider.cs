using Microsoft.Extensions.DependencyInjection;

namespace PresenceMonitor.Utilities.Ioc;

public class SingletonProvider<TService> : ISingletonProvider<TService>
    where TService : class
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IServiceScope? _scope;

    public SingletonProvider(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public bool ScopeActive => this._scope is not null;

    public TService Provide()
    {
        this._scope = this._scope ??= _serviceScopeFactory.CreateScope();
        return this._scope.ServiceProvider.GetRequiredService<TService>();
    }

    public void Dispose()
    {
        this._scope?.Dispose();
        this._scope = null;
    }
}