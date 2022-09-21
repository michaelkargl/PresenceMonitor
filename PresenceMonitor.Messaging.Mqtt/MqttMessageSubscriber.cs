using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using PresenceMonitor.Messaging.Abstractions;
using PresenceMonitor.Messaging.Abstractions.Configuration;

namespace PresenceMonitor.Messaging.Mqtt;

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
            var message = Encoding.UTF8.GetString(mqttMessageReceivedEventArgs.ApplicationMessage.Payload);
            return messageHandlerAsync.Invoke(message, cancellationToken);
        }
        
        this.MqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
        
        await this.ConnectAsync(cancellationToken, this.MqttOptions.Uri);
        await this.SubscribeAsync(cancellationToken, this.MqttOptions.Topic);
    }
    
    private Task SubscribeAsync(CancellationToken cancellationToken, string topic)
    {
        var subscriptionOptions = new MqttFactory()
            .CreateSubscribeOptionsBuilder()
            .WithTopicFilter(topic)
            .Build();
    
        return this.MqttClient.SubscribeAsync(subscriptionOptions, cancellationToken);
    }
    
    private async Task ConnectAsync(CancellationToken cancellationToken, Uri uri)
    {
        var mqttOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(uri.Host, uri.Port)
            .Build();
    
        this._logger.LogDebug("Connecting to MQTT server: {ServerUrl}", uri);
        await this.MqttClient.ConnectAsync(mqttOptions, cancellationToken);
    }
}