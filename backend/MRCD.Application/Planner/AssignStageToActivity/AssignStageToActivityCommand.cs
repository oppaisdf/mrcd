using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.AssignStageToActivity;

public sealed record AssignStageToActivityCommand(
    Guid StageId,
    Guid ActivityId,
    bool IsAssignation,
    bool IsUserMain,
    Guid? UserId,
    string? Notes
) : ICommand<Result>;