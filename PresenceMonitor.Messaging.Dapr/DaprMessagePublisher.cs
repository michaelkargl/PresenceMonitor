using Configuration;
using Dapr.Client;
using Microsoft.Extensions.Options;

public class DaprMessagePublisher : IMessagePublisher
{
    private readonly DaprClient _daprClient;
    private readonly IOptions<DaprPublisherOptions> _options;

    public DaprMessagePublisher(DaprClient daprClient, IOptions<DaprPublisherOptions> options)
    {
        this._daprClient = daprClient;
        this._options = options;
    }

    private DaprPublisherOptions DaprOptions => this._options.Value;

    public Task PublishAsync(object message, CancellationToken cancellationToken = default) =>
        this._daprClient.PublishEventAsync(
            this.DaprOptions.PubSubName,
            this.DaprOptions.Topic,
            message,
            cancellationToken);
}