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
    Task CheckAsync(string userID, string hash, DateTime? date = null);
    Task<ICollection<AttendanceResponse>> GetAsync(string userId, AttendanceFilter filter);
    Task RemoveCheckAsync(string userId, int attendanceId);
    Task<ICollection<QRResponse>> GetQRsAsync(string userId);
    Task<ICollection<GeneralListResponse>> GetListAsync(string userId);
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
        string hash,
        DateTime? date = null
    )
    {
        var person = await _context.People
            .Where(p => p.Hash == hash)
            .Select(p => new
            {
                Id = p.Id!.Value,
                p.IsActive,
                Dates = _context.Attendance
                    .Where(a => a.PersonId == p.Id)
                    .Select(a => a.Date)
                    .ToList()
            })
            .FirstAsync() ?? throw new DoesNotExistsException("El confirmado no existe o está inactivo");

        var dateS = $"{date!.Value.Year}{date!.Value.Month}{date!.Value.Day}";
        var checks = person.Dates == null ? 0 : person.Dates.Where(d => $"{d.Date.Year}{d.Date.Month}{d.Date.Day}" == dateS).Count();

        if (!person.IsActive) throw new DoesNotExistsException("El confirmado no existe o está inactivo");
        if (checks > 0) throw new BadRequestException("Ya se ha pasado asistenncia de este cofirmando");
        _context.Attendance.Add(new Attendance
        {
            UserId = userID,
            PersonId = person.Id,
            IsAttendance = date != null,
            Date = date ?? DateTime.UtcNow.AddHours(-6)
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
                Id = a.Id!.Value,
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
                Id = a.Id,
                User = a.User,
                Person = a.Person,
                Date = a.Date
            })
            .ToListAsync();
    }

    public async Task<ICollection<GeneralListResponse>> GetListAsync(
        string userId
    )
    {
        await _logs.RegisterReadingAsync(userId, "Listado general");
        return await _context.People
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => new GeneralListResponse
            {
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
            .ToListAsync();
    }

    public async Task<ICollection<QRResponse>> GetQRsAsync(
        string userId
    )
    {
        await _logs.RegisterReadingAsync(userId, "Todos los códigos QR");
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