namespace Exceptions;

public class PresenceApiRequestFailedException : Exception
{
    internal static string BuildMessage(string requestName, string reason)
    {
        reason = string.IsNullOrWhiteSpace(reason) ? reason : "None"; 
        return $"PresenceApi service request {requestName} failed due to the following reasons: {reason}";
    }
    
    public PresenceApiRequestFailedException(string requestName, string reason, Exception? inner) : base(
        BuildMessage(requestName, reason), inner
    )
    {
    }
}