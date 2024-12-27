using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Filters;
using api.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IAttendanceService
{
    Task CheckAsync(string userID, int personId, DateTime? date = null);
    Task<ICollection<AttendanceResponse>> GetAsync(string userId, AttendanceFilter filter);
    Task RemoveCheckAsync(string userId, int attendanceId);
}

public class AttendanceService(
    MerContext context,
    ILogService logs,
    UserManager<IdentityUser> users
) : IAttendanceService
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;
    private readonly UserManager<IdentityUser> _users = users;

    public async Task CheckAsync(
        string userID,
        int personId,
        DateTime? date = null
    )
    {
        if (date == null) date = DateTime.UtcNow;
        var dateS = $"{date!.Value.Year}{date!.Value.Month}{date!.Value.Date}";
        var person = await (
            from p in _context.People
            join temp in _context.Attendance on p.Id equals temp.PersonId into tempG
            from a in tempG.DefaultIfEmpty()
            group new { p, a } by p.IsActive into pg
            select new
            {
                IsActive = pg.Key,
                Checks = pg
                    .Where(x => $"{x.a.Date.Year}{x.a.Date.Month}{x.a.Date.Date}" == dateS)
                    .Count()
            }).FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El confirmado no existe o está inactivo");

        if (!person.IsActive) throw new DoesNotExistsException("El confirmado no existe o está inactivo");
        if (person.Checks > 0) throw new BadRequestException("Ya se ha pasado asistenncia de este cofirmando");
        _context.Attendance.Add(new Attendance
        {
            UserId = userID,
            PersonId = personId,
            Date = date!.Value
        });
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<AttendanceResponse>> GetAsync(
        string userId,
        AttendanceFilter filter
    )
    {
        await _logs.RegisterReadingAsync(userId, "Asistencias");
        var users = (await _users.Users.ToListAsync()).ToDictionary(u => u.Id, u => u.UserName);
        var query =
            from a in _context.Attendance
            join p in _context.People on a.PersonId equals p.Id
            orderby a.Date descending
            select new
            {
                User = users[a.UserId]!,
                a.UserId,
                Person = p.Name,
                a.PersonId,
                a.Date
            };

        if (!string.IsNullOrWhiteSpace(filter.UserId)) query = query.Where(a => a.UserId == filter.UserId);
        if (filter.PersonId != null) query = query.Where(a => a.PersonId == filter.PersonId);

        return await query
            .Skip((filter.Page - 1) * 15)
            .Take(15)
            .Select(a => new AttendanceResponse
            {
                User = a.User,
                Person = a.Person,
                Date = a.Date
            })
            .ToListAsync();
    }

    public async Task RemoveCheckAsync(
        string userId,
        int attendanceId
    )
    {
        var check = await _context.Attendance.FindAsync(attendanceId) ?? throw new DoesNotExistsException("El registro no existe");
        _context.Attendance.Remove(check);
        await _context.SaveChangesAsync();
        await _logs.RegisterUpdateAsync(userId, $"Eliminó asistencia {attendanceId} de confirmando {check.PersonId}");
    }
}