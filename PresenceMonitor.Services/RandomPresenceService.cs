using Extensions;

public class RandomPresenceService : IPresenceService
{
    public Task<byte> GetPresenceCountAsync(CancellationToken cancellationToken = default)
        => Random.Shared.NextByteAsync();
}