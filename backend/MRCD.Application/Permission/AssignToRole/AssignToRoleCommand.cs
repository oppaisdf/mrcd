using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.AssignToRole;

public sealed record AssignToRoleCommand(
    Guid UserId,
    Guid PermissionId,
    Guid RoleId
) : ICommand<Result>;