using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Planner.Del;

public sealed record DelActivityCommand(
    Guid UserId,
    Guid ActivityId
) : ICommand<Result>;