using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Attendance.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.Attendance;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.Get.ConsecutiveFouls;

internal sealed class GetConsecutiveFoulsHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    IAttendanceService service
) : IQueryHandler<IReadOnlyCollection<AttendanceDTO>, GetConsecutiveFoulsQuery>
{
    public async Task<Result<IReadOnlyCollection<AttendanceDTO>>> HandleAsync(
        GetConsecutiveFoulsQuery query,
        CancellationToken cancellationToken
    )
    {
        var attendances = await repo.ToListAsync(
            DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
            filteredOnlyByYear: true,
            cancellationToken
        );
        var activePeople = await person.OnlyActiveToListAsync(cancellationToken);
        var results = service.GetAlertAttendances(
            activePeople,
            attendances,
            query.ConsecutiveWeeksCount
        );
        return Result<IReadOnlyCollection<AttendanceDTO>>.Success(results);
    }
}