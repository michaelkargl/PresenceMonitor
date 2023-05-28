namespace Configuration;

public class PresenceApiOptions
{
    public PresenceApiOptions()
    {
    }

    public PresenceApiOptions(string url)
    {
        this.Url = url;
    }

    public string Url { get; set; } = null!;
}