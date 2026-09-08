using MRCD.Application.Attendance.DTOs;

namespace MRCD.Application.Services.Attendance;

internal sealed class AttendanceService : IAttendanceService
{
    public IReadOnlyCollection<AttendanceDTO> GetConsumibleAttendances(
        IReadOnlyCollection<Domain.Person.Person> people,
        IReadOnlyCollection<Domain.Attendance.Attendance> attendances
    )
    {
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

        return [.. people.Select(p => new AttendanceDTO(
            p.Name,
            [.. dates.Select(d => new HasAttendanceDTO(
                d,
                attendanceDir.TryGetValue((p.ID, d), out var a)
                    ? a.IsAttendance
                        ? AttendanceType.Attended
                        : AttendanceType.Excused
                    : AttendanceType.Absent
            ))]
        ))];
    }
}