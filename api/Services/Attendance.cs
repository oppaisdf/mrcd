using api.Common;
using api.Data;
using api.Models.Entities;
using api.Models.Requests;
using api.Models.Responses;

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
    Task CheckAsync(string userID, AttendanceRequest request);

    /// <summary>
    /// Elimina la última asistencia registrada
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="hash">El confirmando debe estar activo</param>
    /// <returns></returns>
    Task UnverifyAsync(string userId, string hash);
    Task<IEnumerable<QRResponse>> GetQRsAsync(string userId);
    Task<IEnumerable<GeneralListResponse>> GetListAsync(string userId);

    /// <summary>
    /// Pasa asistencia a todos los confirmandos de un día
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task CheckAllAsync(string userId, bool day);
    Task<IEnumerable<AttendanceResponse>> GetAsync(string userId);
}

public class AttendanceService(
    ILogService logs,
    IAttendanceRepository repo,
    IPeopleRepository people
) : IAttendanceService
{
    private readonly ILogService _logs = logs;
    private readonly IAttendanceRepository _repo = repo;
    private readonly IPeopleRepository _people = people;

    public async Task CheckAllAsync(
        string userId,
        bool day
    )
    {
        var ids = await _people.IdsByDayAsync(day).ConfigureAwait(false);
        if (!ids.Any()) return;
        await _repo.AddRangeUsingIdsAsync(ids, userId);
    }

    public async Task CheckAsync(
        string userID,
        AttendanceRequest request
    )
    {
        var date = request.Date ?? DateTime.UtcNow.AddHours(-6);
        var personId = await _repo.IdIfNotCheckedAsync(request.Hash, date) ?? throw new DoesNotExistsException("El confirmado no existe, está inactivo o ya se le pasó asistencia");
        var attendance = new Attendance
        {
            UserId = userID,
            PersonId = personId,
            IsAttendance = request.IsAttendance!.Value,
            Date = date
        };
        await _repo.AddAsync(attendance);
    }

    public async Task<IEnumerable<AttendanceResponse>> GetAsync(string userId)
    {
        await _logs.RegisterReadingAsync(userId, "Asistencias");
        return await _repo.ToListAsync();
    }

    public async Task<IEnumerable<GeneralListResponse>> GetListAsync(
        string userId
    )
    {
        await _logs.RegisterReadingAsync(userId, "Listado general");
        return await _repo.GeneralListAsync();
    }

    public async Task<IEnumerable<QRResponse>> GetQRsAsync(
        string userId
    )
    {
        await _logs.RegisterReadingAsync(userId, "Todos los códigos QR");
        return await _people.QRsListAsync();
    }

    public async Task UnverifyAsync(
        string userId,
        string hash
    )
    {
        var attendance = await _repo.LastAttendanceAsync(hash).ConfigureAwait(false);
        if (attendance == null) return;
        await _repo.RemoveAsync(attendance, userId).ConfigureAwait(false);
    }
}