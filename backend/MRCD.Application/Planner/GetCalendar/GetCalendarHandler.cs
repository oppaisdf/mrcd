using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.Contracts;
using MRCD.Application.Planner.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.GetCalendar;

internal sealed class GetCalendarHandler(
    IActivityRepository repo
) : IQueryHandler<IEnumerable<CalendarDTO>, GetCalendarQuery>
{
    private readonly IActivityRepository _repo = repo;

    public async Task<Result<IEnumerable<CalendarDTO>>> HandleAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken
    )
    {
        if (query.Year < 2025 || query.Year > DateTime.UtcNow.Year)
            return Result<IEnumerable<CalendarDTO>>.Failure("El año es incorrecto");
        if (query.Month < 1 || query.Month > 12)
            return Result<IEnumerable<CalendarDTO>>.Failure("El mes no existe");
        var date = new DateOnly(query.Year, query.Month, 1);
        var activities = await _repo.ToListAsync(date, date.AddMonths(1).AddDays(-1), cancellationToken);
        var activitiesDir = activities
            .GroupBy(a => a.Date.Day)
            .ToDictionary(
                a => a.Key,
                a => a.Select(g => new SimpleActivityDTO(
                    g.ID,
                    g.Name
                ))
            );

        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var dayOfWeek = (ushort)date.DayOfWeek;
        var response = new List<CalendarDTO>();

        // Días en blanco
        for (ushort day = 0; day < dayOfWeek; day++)
            response.Add(new(0, []));

        // Días del mes
        for (ushort day = 1; day <= daysInMonth; day++)
            response.Add(new(
                day,
                activitiesDir.TryGetValue(day, out var activity)
                    ? activity
                    : []
            ));

        // Días complementarios
        var remainingDaysInWeek = 7 - (response.Count % 7);
        if (remainingDaysInWeek < 7)
            for (ushort day = 0; day < remainingDaysInWeek; day++)
                response.Add(new(0, []));

        return Result<IEnumerable<CalendarDTO>>.Success(response);
    }
}