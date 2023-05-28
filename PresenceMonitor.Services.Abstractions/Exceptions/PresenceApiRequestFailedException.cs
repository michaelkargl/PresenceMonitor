namespace Exceptions;

public class PresenceApiRequestFailedException : Exception
{
    internal static string BuildMessage(string requestName, string reason) =>
        $"PresenceApi service request {requestName} failed due to the following reasons: {string.IsNullOrWhiteSpace(reason)}";

    public PresenceApiRequestFailedException(string requestName, string reason, Exception? inner) : base(
        BuildMessage(requestName, reason), inner
    )
    {
    }
}