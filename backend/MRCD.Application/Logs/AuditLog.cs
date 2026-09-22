using Microsoft.Extensions.Logging;

namespace MRCD.Application.Logs;

internal sealed class AuditLog<T>(ILogger<T> logger)
{
    public void Write(
        Guid userId,
        string message,
        params object?[] values
    )
    {
        using (logger.BeginScope(new Dictionary<string, object> { ["UserId"] = userId }))
            logger.LogInformation(message, values);
    }
}
