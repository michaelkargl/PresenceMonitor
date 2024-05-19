using Configuration;

// ReSharper disable once UnusedTypeParameter
public interface IMessagePublisher<TMessage, TPublisherOptions> 
    where TPublisherOptions : AbstractDaprPublisherOptions
    where TMessage : notnull
{
    public Task PublishAsync(TMessage message, CancellationToken cancellationToken = default);
}