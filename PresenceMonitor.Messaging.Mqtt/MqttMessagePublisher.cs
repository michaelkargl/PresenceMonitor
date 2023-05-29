using System.Text;
using System.Text.Json;
using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

public class MqttMessagePublisher : IMessagePublisher
{
    private readonly Lazy<IMqttClient> _mqttClient;
    private readonly IOptions<MqttOptions> _mqttOptions;
    private ILogger<MqttMessagePublisher> _logger;

    public MqttMessagePublisher(
        Lazy<IMqttClient> mqttClient,
        ILogger<MqttMessagePublisher> logger,
        IOptions<MqttOptions> mqttOptions
    )
    {
        this._mqttClient = mqttClient;
        this._logger = logger;
        this._mqttOptions = mqttOptions;
    }

    private IMqttClient MqttClient => this._mqttClient.Value;
    private MqttOptions MqttOptions => this._mqttOptions.Value;

    public async Task PublishAsync(object message, CancellationToken cancellationToken)
    {
        await this.ConnectAsync(cancellationToken, this.MqttOptions.Uri);

        var payload = JsonSerializer.Serialize(message);
        await this.MqttClient.PublishAsync(new MqttApplicationMessage
        {
            Topic = this.MqttOptions.Topic,
            PayloadSegment = Encoding.UTF8.GetBytes(payload)
        }, cancellationToken);
    }

    private async Task ConnectAsync(CancellationToken cancellationToken, Uri uri)
    {
        if (this.MqttClient.IsConnected)
        {
            return;
        }

        var mqttOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(uri.Host, uri.Port)
            .Build();

        this._logger.LogDebug("Connecting to MQTT server: {ServerUrl}", uri);
        await this.MqttClient.ConnectAsync(mqttOptions, cancellationToken);
    }
}