using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.DelActivity;

public sealed record DelActivityCommand(
    Guid UserId,
    Guid ActivityId
) : ICommand<Result>;