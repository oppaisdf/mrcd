using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Add.Assign;

public sealed record AssignToRoleCommand(
    Guid UserId,
    Guid PermissionId,
    Guid RoleId
) : ICommand<Result>;