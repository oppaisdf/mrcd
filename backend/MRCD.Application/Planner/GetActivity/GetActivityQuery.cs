using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Planner.DTOs;

namespace MRCD.Application.Planner.GetActivity;

public sealed record GetActivityQuery(
    Guid ActivityId
) : IQuery<ActivityDTO>;