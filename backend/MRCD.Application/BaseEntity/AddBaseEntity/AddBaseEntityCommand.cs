using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.BaseEntity.AddBaseEntity;

public sealed record AddBaseEntityCommand(
    Guid UserId,
    string Name
) : ICommand<Guid>;