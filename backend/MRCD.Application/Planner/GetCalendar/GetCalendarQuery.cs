using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.DTOs;

namespace MRCD.Application.Planner.GetCalendar;

public sealed record GetCalendarQuery(
    ushort Year,
    ushort Month
) : IQuery<IEnumerable<CalendarDTO>>;