using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.Add.Assign;

public sealed record AssignParentCommand(
    Guid ParentId,
    Guid PersonId,
    bool IsParent,
    bool IsAssignation
) : ICommand<Result>;