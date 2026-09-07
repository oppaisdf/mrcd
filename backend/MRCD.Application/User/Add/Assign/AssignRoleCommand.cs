using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Add.Assign;

public sealed record AssignRoleCommand(
    Guid UserId,
    Guid RoleId,
    bool IsAssignment
) : ICommand<Result>;