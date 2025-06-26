using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IPeopleRepository
{
    Task<IEnumerable<int>> IdsByDayAsync(bool day);
    Task<IEnumerable<QRResponse>> QRsListAsync();
}

public class PeopleRepository(
    MerContext context
) : IPeopleRepository
{
    private readonly MerContext _context = context;

    public async Task<IEnumerable<int>> IdsByDayAsync(
        bool day
    )
    {
        var now = DateTime.UtcNow.AddHours(-6);
        var start = now.Date;
        var end = start.AddDays(1);

        return await _context.People
            .AsNoTracking()
            .Where(p => p.IsActive && p.Day == day)
            .Where(p => !_context.Attendance.Any(a =>
                a.PersonId == p.Id &&
                a.Date >= start && a.Date < end))
            .Select(p => p.Id!.Value)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<QRResponse>> QRsListAsync()
    {
        return await _context.People
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Select(p => new QRResponse
            {
                Name = p.Name,
                Day = p.Day,
                Gender = p.Gender,
                Hash = p.Hash
            })
            .ToListAsync();
    }
}