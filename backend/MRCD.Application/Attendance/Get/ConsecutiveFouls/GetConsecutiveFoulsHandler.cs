using MRCD.Application.Abstracts;
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
    IAttendanceService service,
    ICacheService cache
) : IQueryHandler<IReadOnlyCollection<AttendanceDTO>, GetConsecutiveFoulsQuery>
{
    private readonly IAttendanceRepository _repo = repo;
    private readonly IPersonRepository _person = person;
    private readonly IAttendanceService _service = service;
    private readonly ICacheService _cache = cache;

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

    public async Task<Result<IReadOnlyCollection<AttendanceDTO>>> HandleAsync(
        GetConsecutiveFoulsQuery query,
        CancellationToken cancellationToken
    )
    {
        var current = await _cache.GetAsync<IReadOnlyCollection<AttendanceDTO>?>("alert:4", cancellationToken);
        if (current is not null)
            return Result<IReadOnlyCollection<AttendanceDTO>>.Success(current);
        var attendances = await _repo.ToListAsync(
            DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
            filteredOnlyByYear: true,
            cancellationToken
        );
        var activePeople = await _person.OnlyActiveToListAsync(cancellationToken);
        var consecutiveFoulsPeople = ConsecutiveFoulsPeopleToList(
            activePeople,
            attendances,
            query.ConsecutiveWeeksCount
        );
        var results = _service.GetConsumibleAttendances(
            consecutiveFoulsPeople,
            attendances
        );
        await _cache.SetAsync($"alert:4", results, cancellationToken, TimeSpan.FromMinutes(60));

        return Result<IReadOnlyCollection<AttendanceDTO>>.Success(results);
    }
}