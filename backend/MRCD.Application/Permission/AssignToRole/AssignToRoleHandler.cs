using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Role;

namespace MRCD.Application.Permission.AssignToRole;

internal sealed class AssignToRoleHandler(
    IPermissionRepository permission,
    IRoleRepository role,
    IRolePermissionRepository rolePermission,
    ILogger<AssignToRoleHandler> logs,
    IPersistenceContext save,
    IUserRepository user,
    IPermissionCache cache
) : ICommandHandler<AssignToRoleCommand>
{
    private readonly IPermissionRepository _permission = permission;
    private readonly IRoleRepository _role = role;
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly ILogger<AssignToRoleHandler> _logs = logs;
    private readonly IPersistenceContext _save = save;
    private readonly IPermissionCache _cache = cache;
    private readonly IUserRepository _user = user;

    private async Task ClearCache(
        CancellationToken ct
    )
    {
        var users = await _user.ToListAsync(ct);
        var ids = users
            .Where(u => u.IsActive)
            .Select(u => u.ID);
        foreach (var id in ids)
            await _cache.InvalidateAsync(id, ct);
    }

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
        await ClearCache(cancellationToken);
        _logs.LogInformation("Permission {permission} has been assigned to role {role}", command.PermissionId, command.RoleId);
        return Result.Success();
    }
}