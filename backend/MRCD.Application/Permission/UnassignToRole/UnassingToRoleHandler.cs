using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.UnassignToRole;

internal sealed class UnassignToRoleHandler(
    IRolePermissionRepository rolePermission,
    ILogger<UnassignToRoleHandler> logs
) : ICommandHandler<UnassignToRoleCommand>
{
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly ILogger<UnassignToRoleHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        UnassignToRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _rolePermission.AlreadyExists(command.RoleId, command.PermissionId, cancellationToken);
        if (!exists)
            return Result.Failure("El permiso asignado al rol no existe");
        await _rolePermission.DeleteAsync(command.RoleId, command.PermissionId, cancellationToken);
        _logs.LogInformation("The permission {permission} has been removed to role {role}", command.RoleId, command.PermissionId);
        return Result.Success();
    }
}