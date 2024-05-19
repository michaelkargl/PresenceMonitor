namespace Configuration;

public abstract class AbstractDaprPublisherOptions
{
    public bool Enabled { get; set; }
    public string PubSubName { get; set; } = null!;
    public string Topic { get; set; } = null!;
}