using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Domain.Common;
using MRCD.Domain.Role;

namespace MRCD.Application.Permission.Add.Assign;

internal sealed class AssignToRoleHandler(
    IPermissionRepository permission,
    IRoleRepository role,
    IRolePermissionRepository rolePermission,
    AuditLog<AssignToRoleHandler> logs,
    IPersistenceContext save,
    PermissionCacheInvalidator cache
) : ICommandHandler<AssignToRoleCommand>
{
    private readonly IPermissionRepository _permission = permission;
    private readonly IRoleRepository _role = role;
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly AuditLog<AssignToRoleHandler> _logs = logs;
    private readonly IPersistenceContext _save = save;
    private readonly PermissionCacheInvalidator _cache = cache;

    public async Task<Result> HandleAsync(
        AssignToRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists =
            await _role.IdExistsAsync(command.RoleId, cancellationToken)
            && await _permission.IdExistsAsync(command.PermissionId, cancellationToken);
        if (!exists)
            return Result.Failure("El rol o el permiso no existe :c");
        var alreadyExists = await _rolePermission.ExistsAsync(command.RoleId, command.PermissionId, cancellationToken);
        if (alreadyExists)
            return Result.Failure("El permiso no se puede asignar al rol porque ya ha sido asignado :0");
        var rolePermission = new RolePermission(command.RoleId, command.PermissionId);
        _rolePermission.Add(rolePermission);
        await _save.SaveChangesAsync(cancellationToken);
        await _cache.InvalidateActiveUsersAsync(cancellationToken);
        _logs.Write(command.UserId, "Permission {permission} has been assigned to role {role}", command.PermissionId, command.RoleId);
        return Result.Success();
    }
}