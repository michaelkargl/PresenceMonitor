using MediatR;
using Microsoft.Extensions.Logging;
using PresenceMonitor.UseCases.Abstractions;

namespace PresenceMonitor.UseCases;

public class ProcessMessageCommandHandler : AsyncRequestHandler<ProcessMessageCommand>
{
    private readonly ILogger<ProcessMessageCommandHandler> _logger;

    public ProcessMessageCommandHandler(ILogger<ProcessMessageCommandHandler> logger)
    {
        this._logger = logger;
    }

    protected override Task Handle(ProcessMessageCommand request, CancellationToken cancellationToken)
    {
        this._logger.LogInformation("Received message: {Message}", request.Message);
        return Task.CompletedTask;
    }
}