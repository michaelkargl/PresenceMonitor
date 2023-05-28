using MediatR;

public class MonitorPresenceCommandHandler : IRequestHandler<MonitorPresenceCommand>
{
    private readonly IPresenceService _presenceService;
    private readonly IMessagePublisher _messagePublisher;

    public MonitorPresenceCommandHandler(
        IPresenceService presenceService,
        IMessagePublisher messagePublisher
    )
    {
        this._presenceService = presenceService;
        this._messagePublisher = messagePublisher;
    }

    public async Task Handle(MonitorPresenceCommand request, CancellationToken cancellationToken)
    {
        var count = await this._presenceService.GetPresenceCountAsync(cancellationToken);
        await this._messagePublisher.PublishAsync(count, cancellationToken);
    }
}