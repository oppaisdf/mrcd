using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Planner.AddActivity;

public sealed record AddActivityCommand(
    Guid UserId,
    string ActivityName,
    DateOnly Date
) : ICommand<Guid>;