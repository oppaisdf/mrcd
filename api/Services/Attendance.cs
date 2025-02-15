using api.Common;
using api.Context;
using api.Models.Entities;
using api.Models.Repositories;
using api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IAttendanceService
{
    /// <summary>
    /// Registra una asistencia o inasistencia
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="hash">El confirmando debe estar activo</param>
    /// <param name="isAttendance"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task CheckAsync(string userID, string hash, bool isAttendance, DateTime? date = null);

    /// <summary>
    /// Elimina la última asistencia registrada
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="hash">El confirmando debe estar activo</param>
    /// <returns></returns>
    Task UnverifyAsync(string userId, string hash);
    Task<ICollection<QRResponse>> GetQRsAsync(string userId);
    Task<ICollection<GeneralListResponse>> GetListAsync(string userId);

    /// <summary>
    /// Pasa asistencia a todos los confirmandos de un día
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task CheckAllAsync(string userId, bool day);
    Task<IEnumerable<AttendanceResponse>> GetAsync(string userId);
}

public class AttendanceService(
    MerContext context,
    ILogService logs,
    IAttendanceRepository repo
) : IAttendanceService
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;
    private readonly IAttendanceRepository _repo = repo;

    public async Task CheckAllAsync(
        string userId,
        bool day
    )
    {
        var now = DateTime.UtcNow.AddHours(-6);
        var ids = await (
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
                p.Id
        )
        .Distinct()
        .ToListAsync();

        _context.Attendance.AddRange(
            ids.Select(id => new Attendance
            {
                UserId = userId,
                PersonId = id!.Value,
                IsAttendance = true,
                Date = now
            }).ToList()
        );
        await _context.SaveChangesAsync();
    }

    public async Task CheckAsync(
        string userID,
        string hash,
        bool isAttendance,
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
            .FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El confirmado no existe o está inactivo");

        date ??= DateTime.UtcNow.AddHours(-6);
        var dateS = $"{date!.Value.Year}{date!.Value.Month}{date!.Value.Day}";
        var checks = person.Dates == null ? 0 : person.Dates.Where(d => $"{d.Date.Year}{d.Date.Month}{d.Date.Day}" == dateS).Count();

        if (!person.IsActive) throw new DoesNotExistsException("El confirmado no existe o está inactivo");
        if (checks > 0) throw new BadRequestException("Ya se ha pasado asistencia/inasistencia de este cofirmando");
        _context.Attendance.Add(new Attendance
        {
            UserId = userID,
            PersonId = person.Id,
            IsAttendance = isAttendance,
            Date = date!.Value
        });
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttendanceResponse>> GetAsync(string userId)
    {
        await _logs.RegisterReadingAsync(userId, "Asistencias");
        return await _repo.ToListAsync();
    }

    public async Task<ICollection<GeneralListResponse>> GetListAsync(
        string userId
    )
    {
        await _logs.RegisterReadingAsync(userId, "Listado general");
        return await _context.People
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

    public async Task UnverifyAsync(
        string userId,
        string hash
    )
    {
        var id = await (
            from p in _context.People
            join a in _context.Attendance on p.Id equals a.PersonId
            where
                p.Hash == hash
                && p.IsActive
                && a.IsAttendance
            select a
        )
        .OrderByDescending(p => p.Date)
        .FirstOrDefaultAsync() ?? throw new DoesNotExistsException("El confirmando no existe o está inactivo");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Attendance.Remove(id);
            await _context.SaveChangesAsync();
            await _logs.RegisterUpdateAsync(userId, $"Removió asistencia a {id.PersonId}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}