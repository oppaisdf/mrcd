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
        return await (
            from p in _context.People
            join temp in _context.Attendance on p.Id equals temp.PersonId into tempG
            from a in tempG.DefaultIfEmpty()
            where
                p.IsActive &&
                p.Day == day &&
                (
                    (a.Date.Year != now.Year && a.Date.Month != now.Month && a.Date.Day != now.Day)
                    || a == null
                )
            select
                p.Id!.Value
        )
        .Distinct()
        .ToListAsync();
    }

    public async Task<IEnumerable<QRResponse>> QRsListAsync()
    {
        return await _context.People
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