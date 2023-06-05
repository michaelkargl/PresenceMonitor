namespace Configuration;

public class PresenceApiOptions
{
    public PresenceApiOptions()
    {
    }

    public PresenceApiOptions(string appId, string getPresenceCountMethod, string? daprEndpoint = null)
    {
        this.AppId = appId;
        this.GetPresenceCountMethod = getPresenceCountMethod;
        this.DaprEndpoint = daprEndpoint;
    }

    public string? DaprEndpoint { get; set; }
    public string AppId { get; set; } = null!;
    public string GetPresenceCountMethod { get; set; } = null!;
}