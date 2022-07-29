namespace PresenceMonitor.Messaging.Abstractions;

public interface IMessageSubscriber
{
    public Task SubscribeAsync(Func<string, Task> messageHandlerAsync, CancellationToken cancellationToken);
}