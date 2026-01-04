using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.User.AssignRole;

public sealed record AssignRoleCommand(
    Guid UserId,
    Guid RoleId,
    bool IsAssignment
) : ICommand<Result>;