using Configuration;
using Dapr.Client;
using Microsoft.Extensions.Options;
using PresenceMonitor.Utilities.Ioc;

public class DaprMessagePublisher<TMessage, TPublisherOptions> : IMessagePublisher<TMessage, TPublisherOptions>
    where TPublisherOptions : AbstractDaprPublisherOptions
    where TMessage : notnull
{
    private readonly IDaprClientProvider _clientProvider;
    private readonly IOptions<TPublisherOptions> _options;

    public DaprMessagePublisher(IDaprClientProvider clientProvider, IOptions<TPublisherOptions> options)
    {
        this._clientProvider = clientProvider;
        this._options = options;
    }

    private AbstractDaprPublisherOptions Options => this._options.Value;

    public Task PublishAsync(TMessage message, CancellationToken cancellationToken = default) =>
        // We also need to send primitive messages (just numbers)
        // ReSharper disable once HeapView.PossibleBoxingAllocation
        this.PublishMessageAsync(message, this.Options.Topic, cancellationToken);

    private async Task PublishMessageAsync(object message, string topic, CancellationToken cancellationToken)
    {
        using var client = this._clientProvider.Provide();
        await client.PublishEventAsync(
            this.Options.PubSubName,
            topic,
            message,
            cancellationToken);
    }
}