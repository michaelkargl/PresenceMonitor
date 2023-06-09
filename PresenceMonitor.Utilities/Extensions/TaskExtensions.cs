using Microsoft.Extensions.Logging;

namespace PresenceMonitor.Utilities.Extensions;

public static class TaskExtensions
{
    public static async Task SnoreAsync(this Task task, TimeSpan snoreInterval, ILogger logger, byte snoreSize = 8)
    {
        var snoreString = "Z";
        var timer = new PeriodicTimer(snoreInterval);
        do
        {
            logger.LogDebug("{SnoreString}", snoreString);
            await timer.WaitForNextTickAsync();

            snoreString += snoreString.Length % snoreSize == 0
                ? "! Z"
                : "z";
        } while (!task.IsCompleted);

        await task;
    }
}