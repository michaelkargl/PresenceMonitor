using System.Text.Json;
using Configuration;
using Microsoft.Extensions.Logging;

public class FakeMessagePublisher<TMessage, TPublisherOptions> : IMessagePublisher<TMessage, TPublisherOptions> 
    where TPublisherOptions : AbstractDaprPublisherOptions
    where TMessage : notnull
{
    private readonly ILogger<FakeMessagePublisher<TMessage, TPublisherOptions>> _logger;

    public FakeMessagePublisher(ILogger<FakeMessagePublisher<TMessage, TPublisherOptions>> logger)
    {
        this._logger = logger;
    }

    public Task PublishAsync(TMessage message, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(message);
        this._logger.LogDebug("FAKE publishing message {Message}", payload);
        return Task.CompletedTask;
    }
}