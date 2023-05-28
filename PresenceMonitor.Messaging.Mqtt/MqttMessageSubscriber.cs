using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

public class MqttMessageSubscriber : IMessageSubscriber
{
    private readonly ILogger<MqttMessageSubscriber> _logger;
    private readonly IOptions<MqttOptions> _mqttOptions;
    private readonly Lazy<IMqttClient> _mqttClient;

    public MqttMessageSubscriber(
        Lazy<IMqttClient> mqttClient,
        ILogger<MqttMessageSubscriber> logger,
        IOptions<MqttOptions> mqttOptions
    )
    {
        this._mqttClient = mqttClient;
        this._logger = logger;
        this._mqttOptions = mqttOptions;
    }

    private MqttOptions MqttOptions => this._mqttOptions.Value;
    private IMqttClient MqttClient => this._mqttClient.Value;

    public async Task SubscribeAsync(
        Func<string, CancellationToken, Task> messageHandlerAsync,
        CancellationToken cancellationToken
    )
    {
        Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs mqttMessageReceivedEventArgs)
        {
            var message = GetMessage(mqttMessageReceivedEventArgs);
            return messageHandlerAsync.Invoke(message, cancellationToken);
        }

        this.MqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;

        await this.ConnectAsync(this.MqttOptions.Uri, cancellationToken);
        await this.SubscribeAsync(this.MqttOptions.Topic, cancellationToken);
    }

    private static string GetMessage(MqttApplicationMessageReceivedEventArgs mqttMessageReceivedEventArgs)
        => Encoding.UTF8.GetString(mqttMessageReceivedEventArgs.ApplicationMessage.PayloadSegment);

    private Task SubscribeAsync(string topic, CancellationToken cancellationToken)
    {
        var subscriptionOptions = new MqttFactory()
            .CreateSubscribeOptionsBuilder()
            .WithTopicFilter(topic)
            .Build();

        return this.MqttClient.SubscribeAsync(subscriptionOptions, cancellationToken);
    }

    private async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
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