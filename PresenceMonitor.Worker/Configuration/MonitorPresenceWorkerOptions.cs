namespace Configuration;

public class MonitorPresenceWorkerOptions
{
    public MonitorPresenceWorkerOptions()
    {
    }

    public MonitorPresenceWorkerOptions(
        TimeSpan interval,
        TimeSpan startupDelay
    )
    {
        this.Interval = interval;
        this.StartupDelay = startupDelay;
    }

    public TimeSpan StartupDelay { get; set; }
    
    /// <summary>
    /// HH:mm:ss.fffffff
    /// </summary>
    public TimeSpan Interval { get; set; }
}