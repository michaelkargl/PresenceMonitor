namespace Configuration;

public class DaprPublisherOptions
{
    public bool Enabled { get; set; }
    public string? DaprEndpoint { get; set; }
    public string PubSubName { get; set; } = null!;
    public string Topic { get; set; } = null!;
}