using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Attendance.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services;
using MRCD.Application.Services.Attendance;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.Get;

internal sealed class GetAttendaceHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    IAttendanceService service
) : IQueryHandler<IEnumerable<AttendanceDTO>, GetAttendanceQuery>
{
    public async Task<Result<IEnumerable<AttendanceDTO>>> HandleAsync(
        GetAttendanceQuery query,
        CancellationToken cancellationToken
    )
    {
        var normalizedPersonName = string.IsNullOrWhiteSpace(query.PersonName)
            ? null
            : StringNormalizer.NormalizeString(query.PersonName);

        var attendances = await repo.ToListAsync(
            query.Date,
            query.FilteredOnlyByYear,
            cancellationToken
        );

        var rawPeople = await person.OnlyActiveToListAsync(cancellationToken);
        var people = rawPeople
            .Where(p =>
                (query.IsSunday is null || query.IsSunday == p.IsSunday)
                && (query.IsMasculine is null || query.IsMasculine == p.IsMasculine)
                && (normalizedPersonName is null || p.NormalizedName.Contains(normalizedPersonName))
            )
            .ToList();

        return Result<IEnumerable<AttendanceDTO>>.Success(service.GetConsumibleAttendances(
            people,
            attendances
        )
        );
    }
}