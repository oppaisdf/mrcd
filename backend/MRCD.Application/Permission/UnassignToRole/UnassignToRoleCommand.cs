using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.UnassignToRole;

public sealed record UnassignToRoleCommand(
    Guid UserId,
    Guid RoleId,
    Guid PermissionId
) : ICommand<Result>;