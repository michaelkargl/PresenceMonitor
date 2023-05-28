using Configuration;
using Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    internal Task ExecuteOnceAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            this._logger.LogDebug("Triggering {Command} cycle", nameof(MonitorPresenceCommand));
            return mediator.Send(new MonitorPresenceCommand(), cancellationToken);
        }
        catch (Exception ex)
        {
            throw new MonitorPresenceException(ex.Message, ex);
        } 
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._logger.LogDebug("Starting {Worker}", nameof(MonitorPresenceWorker));
        
        var monitorInterval = this.WorkerOptions.Interval;
        var timer = new PeriodicTimer(monitorInterval);
        do
        {
            var mediator = this._serviceCollection.GetRequiredService<IMediator>();
            await this.ExecuteOnceAsync(mediator, stoppingToken);
        } while (await timer.WaitForNextTickAsync(cancellationToken: stoppingToken));
    }
}