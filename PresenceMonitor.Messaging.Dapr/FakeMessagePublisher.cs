using System.Text.Json;
using Microsoft.Extensions.Logging;

public class FakeMessagePublisher : IMessagePublisher
{
    private readonly ILogger<FakeMessagePublisher> _logger;

    public FakeMessagePublisher(ILogger<FakeMessagePublisher> logger)
    {
        this._logger = logger;
    }

    public Task PublishAsync(object message, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(message);
        this._logger.LogDebug("FAKE publishing message {Message}", payload);
        return Task.CompletedTask;
    }
}