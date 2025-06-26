using api.Models.Entities;
using api.Models.Responses;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public interface IAttendanceRepository
{
    Task<IEnumerable<AttendanceResponse>> ToListAsync();
    Task AddRangeUsingIdsAsync(IEnumerable<int> ids, string userId);
    Task AddAsync(Attendance attendance);
    Task<IEnumerable<GeneralListResponse>> GeneralListAsync();
    Task RemoveAsync(Attendance attendance, string userId);
    Task<Attendance?> LastAttendanceAsync(string qr);
    Task<int?> IdIfNotCheckedAsync(string hash, DateTime date);
}

public class AttendanceRepository(
    MerContext context,
    ILogService logs
) : IAttendanceRepository
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;

    public async Task AddAsync(
        Attendance attendance
    )
    {
        _context.Attendance.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeUsingIdsAsync(
        IEnumerable<int> ids,
        string userId
    )
    {
        _context.Attendance.AddRange(
            ids.Select(id => new Attendance
            {
                UserId = userId,
                PersonId = id,
                IsAttendance = true,
                Date = DateTime.UtcNow.AddHours(-6)
            }).ToList()
        );
        await _context.SaveChangesAsync();
    }

    public async Task<int?> IdIfNotCheckedAsync(
        string hash,
        DateTime date
    )
    {
        return await _context.People
            .Where(p =>
                p.Hash == hash && p.IsActive &&
                !_context.Attendance.Any(a => a.PersonId == p.Id && a.Date.Year == date.Year && a.Date.Month == date.Month && a.Date.Day == date.Day)
            )
            .Select(p => p.Id)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<GeneralListResponse>> GeneralListAsync()
    => await _context.People
        .AsNoTracking()
        .Where(p => p.IsActive)
        .Select(p => new GeneralListResponse
        {
            Id = p.Id!.Value,
            Name = p.Name,
            Gender = p.Gender,
            Day = p.Day,
            DOB = p.DOB,
            Phone = p.Phone,
            Parents = (
                from parent in _context.Parents
                join pp in _context.ParentsPeople on parent.Id equals pp.ParentId
                where pp.PersonId == p.Id && pp.IsParent
                select new GeneralParentListResponse
                {
                    Name = parent.Name,
                    Phone = parent.Phone
                }
            ).ToList()
        })
        .ToListAsync()
        .ConfigureAwait(false);

    public async Task RemoveAsync(
        Attendance attendance,
        string userId
    )
    {
        using var tran = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _logs.RegisterUpdateAsync(userId, $"Removió asistencia a {attendance.PersonId}").ConfigureAwait(false);
            await tran.CommitAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            await tran.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IEnumerable<AttendanceResponse>> ToListAsync()
    {
        var currentYear = DateTime.Now.Year;
        var distinctDates = await _context.Attendance
            .AsNoTracking()
            .Where(a => a.Date.Year == currentYear)
            .Select(a => new { a.Date.Month, a.Date.Day })
            .Distinct()
            .ToListAsync();

        var people = await _context.People
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Select(p => new { p.Id, p.Name, p.Day, p.Gender })
            .ToListAsync();

        var attendances = await _context.Attendance
            .AsNoTracking()
            .Where(a => a.Date.Year == currentYear)
            .ToListAsync();

        var query =
            from p in people
            from d in distinctDates
            let att = attendances.SingleOrDefault(a =>
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

        return [.. query
            .GroupBy(x => new { x.Name, x.Day, x.Gender })
            .Select(g => new AttendanceResponse(
                g.Key.Name,
                g.Key.Day,
                g.Key.Gender,
                [.. g.Select(x => x.DateInfo).OrderBy(di => di.Month).ThenBy(di => di.Day)]))
        ];
    }

    public async Task<Attendance?> LastAttendanceAsync(string qr)
    => await _context.Attendance
        .AsNoTracking()
        .Where(a => _context.People.Any(p => p.IsActive && p.Hash == qr && a.PersonId == p.Id))
        .OrderByDescending(a => a.Date)
        .FirstOrDefaultAsync()
        .ConfigureAwait(false);
}