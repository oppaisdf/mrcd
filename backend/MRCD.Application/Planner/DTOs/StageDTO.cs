namespace MRCD.Application.Planner.DTOs;

public sealed record StageDTO(
    Guid StageId,
    string StageName,
    bool IsUserMain,
    Guid? UserId,
    string? Username,
    string? Notes
);