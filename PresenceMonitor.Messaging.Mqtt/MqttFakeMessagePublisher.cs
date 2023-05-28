using System.Text.Json;
using Microsoft.Extensions.Logging;

public class MqttFakeMessagePublisher : IMessagePublisher
{
    private readonly ILogger<MqttFakeMessagePublisher> _logger;

    public MqttFakeMessagePublisher(ILogger<MqttFakeMessagePublisher> logger)
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