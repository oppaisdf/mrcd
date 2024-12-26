using api.Common;
using api.Context;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface ILogService
{
    Task<ICollection<ActionLog>> GetAsync();
    Task CreateAsync(string name);
    Task UpdateAsync(short id, string name);
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

    public async Task CreateAsync(
        string name
    )
    {
        var alreadyExists = await _context.ActionsLog.AnyAsync(a => a.Name == name);
        if (alreadyExists) throw new AlreadyExistsException("La acción ya existe");
        _context.ActionsLog.Add(new ActionLog
        {
            Name = name
        });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        short id,
        string name
    )
    {
        var alreadyExists = await _context.ActionsLog.AnyAsync(a => a.Name == name && a.Id != id);
        if (alreadyExists) throw new AlreadyExistsException("La acción ya existe");
        var action = await _context.ActionsLog.FindAsync(id) ?? throw new DoesNotExistsException("La acción no existe");
        action.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<ActionLog>> GetAsync() =>
        await _context.ActionsLog.ToListAsync();
}