using Configuration;
using MediatR;

public class MonitorPresenceCommandHandler : IRequestHandler<MonitorPresenceCommand>
{
    private readonly IPresenceService _presenceService;
    private readonly IMessagePublisher<byte, RawDaprPublisherOptions> _rawMessagePublisher;

    private readonly IMessagePublisher<PresenceMessageV1, PresenceMessageV1DaprPublisherOptions>
        _presenceMessageV1Publisher;

    public MonitorPresenceCommandHandler(
        IPresenceService presenceService,
        IMessagePublisher<byte, RawDaprPublisherOptions> rawMessagePublisher,
        IMessagePublisher<PresenceMessageV1, PresenceMessageV1DaprPublisherOptions> presenceMessageV1Publisher)
    {
        this._presenceService = presenceService;
        this._rawMessagePublisher = rawMessagePublisher;
        this._presenceMessageV1Publisher = presenceMessageV1Publisher;
    }

    public async Task Handle(MonitorPresenceCommand request, CancellationToken cancellationToken)
    {
        var presence = await GetPresenceAsync(cancellationToken);
        await Task.WhenAll(
            this._presenceMessageV1Publisher.PublishAsync(presence, cancellationToken),
            this._rawMessagePublisher.PublishAsync(presence.presenceCount, cancellationToken)
        );
    }

    private async Task<PresenceMessageV1> GetPresenceAsync(CancellationToken cancellationToken)
    {
        var count = await this._presenceService.GetPresenceCountAsync(cancellationToken);
        return BuildMessage(count);
    }

    private static PresenceMessageV1 BuildMessage(byte count) => new(
        Guid.NewGuid(),
        DateTimeOffset.Now,
        count
    );
}