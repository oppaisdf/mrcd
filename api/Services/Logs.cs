using api.Context;
using api.Models.Entities;

namespace api.Services;

public interface ILogService
{
    Task RegisterReadingAsync(string userId, string? details = null);
    Task RegisterCreationAsync(string userId, string? details = null);
    Task RegisterUpdateAsync(string userId, string? details = null);
}

public class LogService(
    MerContext context
) : ILogService
{
    private readonly MerContext _context = context;

    private async Task InsertLog(
        string userId,
        short actionId,
        string? details = null
    )
    {
        _context.Logs.Add(new Log
        {
            ActionId = actionId,
            UserId = userId!,
            Details = details,
            Date = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }

    public async Task RegisterCreationAsync(
        string userId,
        string? details = null
    ) => await InsertLog(userId, 1, details);

    public async Task RegisterReadingAsync(
        string userId,
        string? details = null
    ) => await InsertLog(userId, 2, details);

    public async Task RegisterUpdateAsync(
        string userId,
        string? details = null
    ) => await InsertLog(userId, 3, details);
}