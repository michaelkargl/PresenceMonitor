using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly IOptions<MqttOptions> _mqttOptions;

    public Application(
        ILogger<Application> logger,
        IOptions<MqttOptions> mqttOptions
    )
    {
        this._logger = logger;
        this._mqttOptions = mqttOptions;
    }
    
    public Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        this._logger.LogInformation("Running...");
        this._logger.LogInformation("Configured MQTT server: {Url}", this._mqttOptions.Value.Url);
        
        // connect to MQTT
        // log messages
        
        return Task.CompletedTask;
    }
}