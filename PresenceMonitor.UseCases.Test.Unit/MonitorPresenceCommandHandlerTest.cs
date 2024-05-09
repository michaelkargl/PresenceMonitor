using Configuration;
using Moq;

public class MonitorPresenceCommandHandlerTest
{
    private readonly Mock<IPresenceService> _presenceServiceMock = new();
    private readonly Mock<IMessagePublisher<byte, RawDaprPublisherOptions>> _rawMessagePublisherMock = new();
    private readonly Mock<IMessagePublisher<PresenceMessageV1, PresenceMessageV1DaprPublisherOptions>> _presenceMessagePublisherMock = new();
    private readonly MonitorPresenceCommandHandler _commandHandler;

    public MonitorPresenceCommandHandlerTest()
    {
        this._commandHandler = new MonitorPresenceCommandHandler(
            this._presenceServiceMock.Object,
            this._rawMessagePublisherMock.Object,
            this._presenceMessagePublisherMock.Object);
    }


    [Fact]
    public async Task Handle_IfSuccessful_PublishesPresenceMessage()
    {
        const byte expectedPresenceCount = 123;
        this.SetGetPresenceCountAsyncResult(expectedPresenceCount);

        await this._commandHandler.Handle(new MonitorPresenceCommand(), CancellationToken.None);

        this.VerifyGetPresenceCountAsyncCalled(Times.Once());
        this.VerifyPublishMessageAsyncCalled(Times.Once(), expectedPresenceCount);
    }

    private void SetGetPresenceCountAsyncResult(byte expectedCount) => this._presenceServiceMock
        .Setup(service => service.GetPresenceCountAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedCount);

    private void VerifyPublishMessageAsyncCalled(Times times, byte expectedMessage) =>
        this._rawMessagePublisherMock.Verify(publisher => publisher.PublishAsync(
                expectedMessage,
                It.IsAny<CancellationToken>()
            ),
            times);

    private void VerifyGetPresenceCountAsyncCalled(Times times) => this._presenceServiceMock.Verify(service =>
        service.GetPresenceCountAsync(
            It.IsAny<CancellationToken>()
        ), times);
}