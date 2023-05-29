using Configuration;
using Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

public class MonitoringWorkerTest : IAsyncLifetime
{
    private readonly Mock<IMediator> _mediatorMock = new ();
    private readonly MonitorPresenceWorker _worker;
    private readonly ServiceProvider _serviceCollection;

    public MonitoringWorkerTest()
    {
        this._serviceCollection = new ServiceCollection()
            .AddSingleton(this._mediatorMock.Object)
            .BuildServiceProvider();

        this._worker = new MonitorPresenceWorker(
            this._serviceCollection,
            new Mock<IOptions<MonitorPresenceWorkerOptions>>().Object,
            new NullLogger<MonitorPresenceWorker>());
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await this._serviceCollection.DisposeAsync();

    [Fact]
    public async Task ExecuteOnceAsync_OnSuccess_CallsCommand()
    {
        await this._worker.ExecuteOnceAsync(this._mediatorMock.Object, CancellationToken.None);
        this.AssertCommandCalled<MonitorPresenceCommand>(Times.Once());
    }

    [Fact]
    public async Task ExecuteOnceAsync_OnException_Throws()
    {
        var exception = new MonitorPresenceException("Expected test exception");
        var expectedMessage = MonitorPresenceException.BuildMessage(exception.Reason);
        this.SetThrowingCommandInvocation<MonitorPresenceCommand>(exception);

        var actualException = await Assert.ThrowsAsync<MonitorPresenceException>(() =>
            this._worker.ExecuteOnceAsync(this._mediatorMock.Object, CancellationToken.None)
        );

        actualException?.Message.Should().NotBeNull();
        actualException!.Message.Should().Be(expectedMessage);
    }

    private void AssertCommandCalled<TCommand>(Times times) where TCommand : IRequest =>
        this._mediatorMock.Verify(m => m.Send(
            It.IsAny<TCommand>(),
            It.IsAny<CancellationToken>()
        ), times);

    private void SetThrowingCommandInvocation<TCommand>(
        Exception exception
    ) where TCommand : IRequest => this._mediatorMock
        .Setup(m => m.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(exception);
}