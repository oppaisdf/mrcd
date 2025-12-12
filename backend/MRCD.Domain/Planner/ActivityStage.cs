namespace MRCD.Domain.Planner;

public sealed record ActivityStage(
    Guid ActivityId,
    Guid StageId,
    bool IsUserMain,
    Guid? UserId,
    string? Notes
);