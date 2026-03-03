using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Security;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.UnassignToRole;

internal sealed class UnassignToRoleHandler(
    IRolePermissionRepository rolePermission,
    ILogger<UnassignToRoleHandler> logs,
    IUserRepository user,
    IPermissionCache cache
) : ICommandHandler<UnassignToRoleCommand>
{
    private readonly IRolePermissionRepository _rolePermission = rolePermission;
    private readonly ILogger<UnassignToRoleHandler> _logs = logs;
    private readonly IUserRepository _user = user;
    private readonly IPermissionCache _cache = cache;

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
        UnassignToRoleCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _rolePermission.ExistsAsync(command.RoleId, command.PermissionId, cancellationToken);
        if (!exists)
            return Result.Failure("El permiso asignado al rol no existe");
        await _rolePermission.DeleteAsync(command.RoleId, command.PermissionId, cancellationToken);
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("The permission {permission} has been removed from role {role}", command.PermissionId, command.RoleId);
        }
        await ClearCache(cancellationToken);
        return Result.Success();
    }
}