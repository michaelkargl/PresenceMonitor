using MediatR;
using Microsoft.Extensions.Logging;

public class ProcessMessageCommandHandler : IRequestHandler<ProcessMessageCommand>
{
    private readonly ILogger<ProcessMessageCommandHandler> _logger;

    public ProcessMessageCommandHandler(ILogger<ProcessMessageCommandHandler> logger)
    {
        this._logger = logger;
    }
    
    public Task Handle(ProcessMessageCommand request, CancellationToken cancellationToken)
    {
        this._logger.LogInformation("Received message: {Message}", request.Message);
        return Task.CompletedTask;
    }
}