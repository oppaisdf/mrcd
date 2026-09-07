using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.BaseEntity.Add;

public sealed record AddBaseEntityCommand(
    Guid UserId,
    string Name
) : ICommand<Guid>;