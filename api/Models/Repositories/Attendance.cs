using api.Context;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Repositories;

public interface IAttendanceRepository
{
    Task<IEnumerable<AttendanceResponse>> ToListAsync();
}

public class AttendanceRepository(
    MerContext context
) : IAttendanceRepository
{
    private readonly MerContext _context = context;

    public async Task<IEnumerable<AttendanceResponse>> ToListAsync()
    {
        var currentYear = DateTime.Now.Year;
        var distinctDates = await _context.Attendance
            .Where(a => a.Date.Year == currentYear)
            .Select(a => new { a.Date.Month, a.Date.Day })
            .Distinct()
            .ToListAsync();

        var people = await _context.People
            .Where(p => p.IsActive)
            .Select(p => new { p.Id, p.Name, p.Day, p.Gender })
            .ToListAsync();

        var attendances = await _context.Attendance
            .Where(a => a.Date.Year == currentYear)
            .ToListAsync();

        var query =
            from p in people
            from d in distinctDates
            let att = attendances.FirstOrDefault(a =>
                a.PersonId == p.Id &&
                a.Date.Month == d.Month &&
                a.Date.Day == d.Day)
            select new
            {
                p.Name,
                p.Day,
                p.Gender,
                DateInfo = new DateAttendanceResponse(
                    d.Day,
                    d.Month,
                    att?.IsAttendance)
            };

        var grouped = query
            .GroupBy(x => new { x.Name, x.Day, x.Gender })
            .Select(g => new AttendanceResponse(
                g.Key.Name,
                g.Key.Day,
                g.Key.Gender,
                g.Select(x => x.DateInfo).OrderBy(di => di.Month).ThenBy(di => di.Day).ToList()))
            .ToList();

        return grouped;
    }
}