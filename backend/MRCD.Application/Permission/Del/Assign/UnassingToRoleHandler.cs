using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Del.Assign;

internal sealed class UnassignToRoleHandler(
    IRolePermissionRepository rolePermission,
    AuditLog<UnassignToRoleHandler> logs,
    PermissionCacheInvalidator cache
) : ICommandHandler<UnassignToRoleCommand>
{
    public async Task<Result> HandleAsync(
        UnassignToRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await rolePermission.ExistsAsync(
            command.RoleId,
            command.PermissionId,
            cancellationToken
        );
        if (!exists)
            return Result.Failure("El permiso asignado al rol no existe");
        await rolePermission.DeleteAsync(
            command.RoleId,
            command.PermissionId,
            cancellationToken
        );
        logs.Write(command.UserId, "The permission {permission} has been removed from role {role}", command.PermissionId, command.RoleId);
        await cache.InvalidateActiveUsersAsync(cancellationToken);
        return Result.Success();
    }
}