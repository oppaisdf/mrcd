using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Add.Assign;

public sealed record AssignPersonEntityCommand(
    Guid PersonId,
    Guid EntityId,
    bool IsAssignation,
    BaseEntity.Common.BaseEntityType Entity
) : ICommand<Result>;