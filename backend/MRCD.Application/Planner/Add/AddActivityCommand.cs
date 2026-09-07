using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Planner.Add;

public sealed record AddActivityCommand(
    Guid UserId,
    string ActivityName,
    DateOnly Date
) : ICommand<Guid>;