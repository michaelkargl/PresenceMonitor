public interface IMessagePublisher
{
    public Task PublishAsync(object message, CancellationToken cancellationToken = default);
}