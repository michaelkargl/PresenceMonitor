public interface IPresenceService
{
    Task<byte> GetPresenceCountAsync(CancellationToken cancellationToken = default);
}