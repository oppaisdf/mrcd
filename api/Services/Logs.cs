using api.Context;
using api.Models.Entities;
using api.Models.Filters;
using api.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface ILogService
{
    Task<(ICollection<LogResponse> response, string pages)> GetAsync(LogFilter filter);
    Task<FilterResponse> GetFiltersAsync();
    Task RegisterReadingAsync(string userId, string? details = null);
    Task RegisterCreationAsync(string userId, string? details = null);
    Task RegisterUpdateAsync(string userId, string? details = null);
}

public class LogService(
    MerContext context,
    UserManager<IdentityUser> users
) : ILogService
{
    private readonly MerContext _context = context;
    private readonly UserManager<IdentityUser> _users = users;

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

    public async Task<(ICollection<LogResponse> response, string pages)> GetAsync(
        LogFilter filter
    )
    {
        var users = (await _users.Users.ToListAsync()).ToDictionary(x => x.Id, x => x.UserName);
        var query =
            from l in _context.Logs
            join a in _context.ActionsLog on l.ActionId equals a.Id
            orderby l.Date descending
            select new
            {
                Id = l.Id!.Value,
                User = users[l.UserId]!,
                l.UserId,
                l.Date,
                Action = a.Name,
                ActionId = a.Id,
                l.Details
            };

        if (filter.Action != null) query = query.Where(l => l.ActionId == filter.Action);
        if (!string.IsNullOrEmpty(filter.UserId)) query = query.Where(l => l.UserId == filter.UserId);
        if (filter.Start != null && filter.End != null) query = query.Where(l => l.Date >= filter.Start && l.Date < filter.End);

        var first = await query.FirstOrDefaultAsync();
        var lastPage = (short)Math.Ceiling((first == null ? 0 : first.Id) / 15.0);
        if (filter.Page < 1) filter.Page = 1;
        if (filter.Page > lastPage) filter.Page = lastPage;

        return (await query
            .Skip((filter.Page - 1) * 15)
            .Take(15)
            .Select(l => new LogResponse
            {
                User = l.User,
                Date = l.Date,
                Action = l.Action,
                Details = l.Details
            })
            .ToListAsync(),
            $"{filter.Page}/{lastPage}"
        );
    }

    public async Task<FilterResponse> GetFiltersAsync()
    {
        var actions = await _context.ActionsLog.Select(a => new ActionFilterResponse
        {
            Id = a.Id!.Value,
            Name = a.Name
        }).ToListAsync();

        var users = await _users.Users.Select(u => new UserFilterResponse
        {
            Id = u.Id,
            Name = u.UserName!
        }).ToListAsync();

        return new FilterResponse
        {
            Users = users,
            Actions = actions
        };
    }
}