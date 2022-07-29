using PresenceMonitor.Messaging.Abstractions;

namespace Subscriber;

public class PassThruMessageSubscriber : IMessageSubscriber
{
    private readonly string[] _messages;

    public PassThruMessageSubscriber(string[] messages)
    {
        this._messages = messages;
    }
    
    public async Task SubscribeAsync(Func<string, CancellationToken, Task> messageHandlerAsync, CancellationToken cancellationToken)
    {
        foreach (var message in this._messages)
        {
            await messageHandlerAsync.Invoke(message, cancellationToken);
        }
    }
}