using MRCD.Domain.Common;

namespace MRCD.Domain.Planner;

public sealed record ActivityStage(
    Guid ActivityId,
    Guid StageId,
    bool IsUserMain,
    Guid? UserId,
    string? Notes
)
{
    public static Result<ActivityStage> Create(
        Guid activityId,
        Guid stageId,
        bool isUserMain,
        Guid? userId,
        string? notes
    )
    {
        notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        if (notes?.Length > 50)
            return Result<ActivityStage>.Failure("Las nota no puede exceder los 50 caracteres");
        return Result<ActivityStage>.Success(new(activityId, stageId, isUserMain, userId, notes));
    }
}
