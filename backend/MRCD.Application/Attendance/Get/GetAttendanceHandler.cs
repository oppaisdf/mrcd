using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Attendance.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.Common;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.Get;

internal sealed class GetAttendaceHandler(
    IAttendanceRepository repo,
    ICommonService service,
    IPersonRepository person
) : IQueryHandler<IEnumerable<AttendanceDTO>, GetAttendanceQuery>
{
    private readonly IAttendanceRepository _repo = repo;
    private readonly ICommonService _service = service;
    private readonly IPersonRepository _person = person;

    public async Task<Result<IEnumerable<AttendanceDTO>>> HandleAsync(
        GetAttendanceQuery query,
        CancellationToken cancellationToken
    )
    {
        var normalizedPersonName = string.IsNullOrWhiteSpace(query.PersonName)
            ? null
            : _service.NormalizeString(query.PersonName);

        var attendances = await _repo.ToListAsync(
            query.Date,
            query.FilteredOnlyByYear,
            cancellationToken
        );

        var rawPeople = await _person.OnlyActiveToListAsync(cancellationToken);
        var people = rawPeople
            .Where(p =>
                (query.IsSunday is null || query.IsSunday == p.IsSunday)
                && (query.IsMasculine is null || query.IsMasculine == p.IsMasculine)
                && (normalizedPersonName is null || p.NormalizedName.Contains(normalizedPersonName))
            )
            .ToList();

        var peopleIds = people
            .Select(p => p.ID)
            .ToHashSet();

        var dates = attendances
            .Where(a => peopleIds.Contains(a.PersonId))
            .Select(a => a.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        var attendanceDir = attendances
            .GroupBy(a => (a.PersonId, a.Date))
            .ToDictionary(g => g.Key, g => g.Single());

        return Result<IEnumerable<AttendanceDTO>>.Success(
            people.Select(p => new AttendanceDTO(
                p.Name,
                dates.Select(d => new HasAttendanceDTO(
                    d,
                    attendanceDir.TryGetValue((p.ID, d), out var a)
                        ? a.IsAttendance
                            ? AttendanceType.Attended
                            : AttendanceType.Excused
                        : AttendanceType.Absent
                ))
            ))
        );
    }
}