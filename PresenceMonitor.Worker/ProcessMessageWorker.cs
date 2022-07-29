using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PresenceMonitor.Messaging.Abstractions;

namespace PresenceMonitor.Worker;

public class ProcessMessageWorker : BackgroundService
{
    private readonly ILogger<ProcessMessageWorker> _logger;
    private readonly IMessageSubscriber _messageSubscriber;

    public ProcessMessageWorker(
        ILogger<ProcessMessageWorker> logger,
        IMessageSubscriber messageSubscriber
    )
    {
        this._logger = logger;
        this._messageSubscriber = messageSubscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._logger.LogInformation("Subscribing to messages...");
        await this._messageSubscriber.SubscribeAsync(this.HandleMessageReceivedAsync, stoppingToken);
    }

    private Task HandleMessageReceivedAsync(string message)
    {
        this._logger.LogInformation("Received message: {Message}", message);
        return Task.CompletedTask;
    }
}