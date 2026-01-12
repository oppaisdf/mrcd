namespace MRCD.API.DTOs;

public sealed record AddActivityRequest(
    string Name,
    DateOnly Date
);

public sealed record AssignStageToActivityRequest(
    Guid StageId,
    Guid ActivityId,
    bool IsUserMain = false,
    Guid? UserId = null,
    string? Notes = null
);