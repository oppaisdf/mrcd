using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.AssignPersonEntity;

public sealed record AssignPersonEntityCommand(
    Guid PersonId,
    Guid EntityId,
    bool IsAssignation,
    PersonTypeEntity Entity
) : ICommand<Result>;