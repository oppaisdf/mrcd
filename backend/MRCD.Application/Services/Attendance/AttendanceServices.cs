using MRCD.Application.Attendance.DTOs;

namespace MRCD.Application.Services.Attendance;

internal sealed class AttendanceService : IAttendanceService
{
    private static DateOnly GetStartOfWeek(
        DateOnly date
    )
    {
        int diff = ((int)date.DayOfWeek + 6) % 7;
        return date.AddDays(-diff);
    }

    private static IReadOnlyCollection<Domain.Person.Person> ConsecutiveFoulsPeopleToList(
        IReadOnlyCollection<Domain.Person.Person> people,
        IReadOnlyCollection<Domain.Attendance.Attendance> attendances,
        ushort consecutiveWeeksCount
    )
    {
        var weeks = attendances
            .GroupBy(a => GetStartOfWeek(a.Date))
            .Select(g => new
            {
                Date = g.Key,
                Ids = g.Select(gx => gx.PersonId).ToArray()
            }).OrderByDescending(r => r.Date)
            .Take(consecutiveWeeksCount)
            .ToArray();
        if (weeks.Length < consecutiveWeeksCount) return [];

        var attendedIds = weeks
            .SelectMany(r => r.Ids)
            .Distinct()
            .ToHashSet();
        return [.. people.Where(p => !attendedIds.Contains(p.ID))];
    }

    public IReadOnlyCollection<AttendanceDTO> GetAlertAttendances(
        IReadOnlyCollection<Domain.Person.Person> people,
        IReadOnlyCollection<Domain.Attendance.Attendance> attendances,
        ushort consecutiveFoulsWeekCount
    )
    {
        var consecutiveFoulsPeople = ConsecutiveFoulsPeopleToList(
            people,
            attendances,
            consecutiveFoulsWeekCount
        );
        return GetConsumibleAttendances(
            consecutiveFoulsPeople,
            attendances
        );
    }

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