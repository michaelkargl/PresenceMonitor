namespace Exceptions;

public class MonitorPresenceException : Exception
{
    internal static string BuildMessage(string reason)
        => $"Presence monitoring failed with the following error: {reason}";

    public MonitorPresenceException(string reason, Exception? inner = null) : base(BuildMessage(reason), inner)
    {
        this.Reason = reason;
    }
    
    public string Reason { get; }
}