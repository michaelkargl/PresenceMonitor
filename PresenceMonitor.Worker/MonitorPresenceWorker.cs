using Configuration;
using Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PresenceMonitor.Utilities.Extensions;

public class MonitorPresenceWorker : BackgroundService
{
    private readonly IServiceProvider _serviceCollection;
    private readonly IOptions<MonitorPresenceWorkerOptions> _options;
    private readonly ILogger<MonitorPresenceWorker> _logger;

    public MonitorPresenceWorker(
        IServiceProvider serviceCollection,
        IOptions<MonitorPresenceWorkerOptions> options,
        ILogger<MonitorPresenceWorker> logger
    )
    {
        this._serviceCollection = serviceCollection;
        this._options = options;
        this._logger = logger;
    }

    private MonitorPresenceWorkerOptions WorkerOptions => this._options.Value;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return this.InternalExecuteAsync(stoppingToken);
    }

    internal async Task InternalExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await this.DelayStartupAsync(this.WorkerOptions.StartupDelay, cancellationToken);
            await this.StartMonitoring(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MonitorPresenceException(ex.Message, ex);
        }
    }

    private async Task DelayStartupAsync(TimeSpan startupDelay, CancellationToken cancellationToken)
    {
        this._logger.LogDebug("Awaiting startup delay of {Milliseconds} ms", startupDelay.TotalMilliseconds);
        await Task
            .Delay(startupDelay, cancellationToken)
            .SnoreAsync(TimeSpan.FromSeconds(1), this._logger);
    }

    private async Task StartMonitoring(CancellationToken cancellationToken)
    {
        this._logger.LogDebug("Starting {Worker}", nameof(MonitorPresenceWorker));

        var monitorInterval = this.WorkerOptions.Interval;
        var timer = new PeriodicTimer(monitorInterval);
        do
        {
            this._logger.LogDebug("Triggering {Command} cycle", nameof(MonitorPresenceCommand));
            var mediator = this._serviceCollection.GetRequiredService<IMediator>();
            await mediator.Send(new MonitorPresenceCommand(), cancellationToken);

            this._logger.LogDebug("Awaiting next cycle in {Milliseconds} ms...", monitorInterval.TotalMilliseconds);
        } while (await timer.WaitForNextTickAsync(cancellationToken));
    }
}