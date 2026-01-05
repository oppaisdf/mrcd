namespace MRCD.Application.Planner.DTOs;

public sealed record SimpleActivityDTO(
    Guid ActivityId,
    string ActivityName
);

public sealed record ActivityDTO(
    Guid ActivityId,
    string ActivityName,
    DateOnly Date,
    IEnumerable<StageDTO> Stages
);