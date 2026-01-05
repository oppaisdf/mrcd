namespace MRCD.Application.Planner.DTOs;

public sealed record CalendarDTO(
    ushort Day,
    IEnumerable<SimpleActivityDTO> Activities
);