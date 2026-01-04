namespace MRCD.Application.Attendance.Contracts;

public interface IAttendanceRepository
{
    void Add(Domain.Attendance.Attendance attendance);
    Task<bool> AlreadyExistsAsync(Domain.Attendance.Attendance attendance, CancellationToken cancellationToken);
    Task DeleteAsync(Guid personId, DateOnly date, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid personId, DateOnly date, CancellationToken cancellationToken);
    Task<List<Domain.Attendance.Attendance>> ToListAsync(
        DateOnly date,
        bool filteredOnlyByYear,
        CancellationToken cancellationToken
    );
}