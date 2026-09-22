using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Del.Assign;

internal sealed class UnassignToRoleHandler(
    IRolePermissionRepository rolePermission,
    AuditLog<UnassignToRoleHandler> logs,
) : ICommandHandler<UnassignToRoleCommand>
{
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly AuditLog<UnassignToRoleHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        UnassignToRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _rolePermission.ExistsAsync(command.RoleId, command.PermissionId, cancellationToken);
        if (!exists)
            return Result.Failure("El permiso asignado al rol no existe");
        await _rolePermission.DeleteAsync(command.RoleId, command.PermissionId, cancellationToken);
        _logs.Write(command.UserId, "The permission {permission} has been removed from role {role}", command.PermissionId, command.RoleId);
        return Result.Success();
    }
}