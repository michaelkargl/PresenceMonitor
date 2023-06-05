namespace Configuration;

public class MonitorPresenceWorkerOptions
{
    public MonitorPresenceWorkerOptions()
    {
    }

    public MonitorPresenceWorkerOptions(TimeSpan interval)
    {
        this.Interval = interval;
    }

    /// <summary>
    /// HH:mm:ss.fffffff
    /// </summary>
    public TimeSpan Interval { get; set; }
}