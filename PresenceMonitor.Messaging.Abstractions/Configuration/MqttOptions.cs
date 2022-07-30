namespace PresenceMonitor.Messaging.Abstractions.Configuration;

public class MqttOptions
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    public string Url { get; set; } = null!;

    public string Topic { get; set; } = null!;
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}