using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PresenceMonitor.UseCases.Abstractions;
using PresenceMonitor.Worker;
using Subscriber;

public class WorkerTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Mock<IMediator> _mediatorMock = new ();

    public WorkerTests()
    {
        this._serviceProvider = new ServiceCollection()
            .AddSingleton(this._mediatorMock.Object)
            .BuildServiceProvider();
    }


    [Theory]
    [InlineData]
    [InlineData("message1")]
    [InlineData("message1", "message2")]
    public async Task StartAsync_OnMessageReceived_CallsProcessMessageCommand(params string[] messages)
    {
        var subscriber = new PassThruMessageSubscriber(messages);
        var worker = new ProcessMessageWorker(
            new Mock<ILogger<ProcessMessageWorker>>().Object,
            subscriber,
            this._serviceProvider
        );

        await worker.StartAsync(CancellationToken.None);

        this._mediatorMock.Verify(m => m.Send(
            It.IsAny<ProcessMessageCommand>(),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(messages.Length));
    }
}