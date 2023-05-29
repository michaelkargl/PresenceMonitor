namespace Configuration;

public class MqttOptions
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    public string Url { get; set; } = null!;

    public string Topic { get; set; } = null!;

    public ushort Port { get; set; }
    
    public Uri Uri => new UriBuilder(this.Url)
    {
        Port = this.Port
    }.Uri;


    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}