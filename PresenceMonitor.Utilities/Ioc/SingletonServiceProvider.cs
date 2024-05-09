namespace PresenceMonitor.Utilities.Ioc;

public interface ISingletonProvider<out TService> : IDisposable
    where TService : class
{
    public bool ScopeActive { get; }
    public TService Provide();
}