namespace PresenceMonitor.Messaging.Abstractions;

public interface IMessageSubscriber
{
    public Task SubscribeAsync(
        Func<string, CancellationToken, Task> messageHandlerAsync,
        CancellationToken cancellationToken
    );
}