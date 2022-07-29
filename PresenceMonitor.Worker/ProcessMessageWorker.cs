using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PresenceMonitor.Messaging.Abstractions;
using PresenceMonitor.UseCases.Abstractions;

namespace PresenceMonitor.Worker;

public class ProcessMessageWorker : BackgroundService
{
    private readonly ILogger<ProcessMessageWorker> _logger;
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IServiceProvider _serviceProvider;

    public ProcessMessageWorker(
        ILogger<ProcessMessageWorker> logger,
        IMessageSubscriber messageSubscriber,
        IServiceProvider serviceProvider
    )
    {
        this._logger = logger;
        this._messageSubscriber = messageSubscriber;
        this._serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._logger.LogInformation("Subscribing to messages...");
        await this._messageSubscriber.SubscribeAsync(this.HandleMessageReceivedAsync, stoppingToken);
    }
    
    private async Task HandleMessageReceivedAsync(string message, CancellationToken cancellationToken)
    {
        await using var scope = this._serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
        await mediator.Send(new ProcessMessageCommand(message), cancellationToken);
    }

    
}