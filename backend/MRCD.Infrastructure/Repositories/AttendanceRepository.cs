using Microsoft.EntityFrameworkCore;
using MRCD.Application.Attendance.Contracts;
using MRCD.Domain.Attendance;

namespace MRCD.Infrastructure.Repositories;

internal sealed class AttendanceRepository(
    Persistence.AppContext app
) : IAttendanceRepository
{
    public void Add(
        Attendance attendance
    ) => app
        .Attendances
        .Add(attendance);

    public Task<bool> AlreadyExistsAsync(
        Attendance attendance,
        CancellationToken cancellationToken
    ) => app
        .Attendances
        .AnyAsync(a =>
            a.Date == attendance.Date
            && a.PersonId == attendance.PersonId,
            cancellationToken
        );

    public Task DeleteAsync(
        Guid personId,
        DateOnly date,
        CancellationToken cancellationToken
    ) => app
        .Attendances
        .Where(a =>
            a.PersonId == personId
            && a.Date == date
        ).ExecuteDeleteAsync(cancellationToken);

    public Task<bool> ExistsAsync(
        Guid personId,
        DateOnly date,
        CancellationToken cancellationToken
    ) => app
        .Attendances
        .AnyAsync(a =>
            a.PersonId == personId
            && a.Date == date,
            cancellationToken
        );

    public Task<List<Attendance>> ToListAsync(
        DateOnly date,
        bool filteredOnlyByYear,
        CancellationToken cancellationToken
    ) => app
        .Attendances
        .AsNoTracking()
        .Where(a =>
            a.Date.Year == date.Year
            && (filteredOnlyByYear || a.Date.Month == date.Month)
        )
        .OrderBy(a => a.Date)
        .ToListAsync(cancellationToken);
}