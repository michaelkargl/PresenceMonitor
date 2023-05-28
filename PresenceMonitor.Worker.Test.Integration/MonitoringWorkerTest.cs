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
            MockMonitorPresenceWorkerOptions(TimeSpan.FromMilliseconds(10)).Object,
            new NullLogger<MonitorPresenceWorker>());
    }


    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await this._serviceCollection.DisposeAsync();

    [Fact]
    public async Task ExecuteOnceAsync_OnSuccess_CallsCommand()
    {
        try
        {
            await this.RunWorkerUntil(TimeSpan.FromMilliseconds(100));
        }
        catch (OperationCanceledException)
        {
        }

        this.AssertCommandCalledAtLeastOnce<MonitorPresenceCommand>();
    }


    [Fact]
    public async Task ExecuteAsync_OnException_Throws()
    {
        var exception = new Exception("Expected test exception");
        var expectedMessage = MonitorPresenceException.BuildMessage(exception.Message);
        this.SetThrowingCommandInvocation<MonitorPresenceCommand>(exception);

        var actualException = await Assert.ThrowsAsync<MonitorPresenceException>(() =>
            this.RunWorkerUntil(TimeSpan.FromSeconds(1))
        );

        actualException.Message.Should().NotBeNull();
        actualException!.Message.Should().Be(expectedMessage);
    }

    private static IMock<IOptions<MonitorPresenceWorkerOptions>> MockMonitorPresenceWorkerOptions(TimeSpan interval)
    {
        var mock = new Mock<IOptions<MonitorPresenceWorkerOptions>>();
        mock
            .Setup(m => m.Value)
            .Returns(new MonitorPresenceWorkerOptions(interval));
        return mock;
    }

    private void AssertCommandCalledAtLeastOnce<TCommand>() where TCommand : IRequest
        => this.AssertCommandCalled<TCommand>(Times.AtLeastOnce);

    private void AssertCommandCalled<TCommand>(Func<Times> times) where TCommand : IRequest =>
        this._mediatorMock.Verify(m => m.Send(
            It.IsAny<TCommand>(),
            It.IsAny<CancellationToken>()
        ), times);

    private void SetThrowingCommandInvocation<TCommand>(
        Exception exception
    ) where TCommand : IRequest => this._mediatorMock
        .Setup(m => m.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(exception);

    private async Task RunWorkerUntil(TimeSpan deadline)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var task = this._worker.InternalExecuteAsync(cancellationTokenSource.Token);
        cancellationTokenSource.CancelAfter(deadline);
        await task;
    }
}