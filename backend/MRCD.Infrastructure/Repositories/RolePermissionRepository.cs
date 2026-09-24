using Microsoft.EntityFrameworkCore;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Repositories;

internal sealed class RolePermissionRepository(
    Persistence.AppContext app
) : IRolePermissionRepository
{
    public void Add(
        RolePermission rolePermission
    ) => app
        .RolesPermissions
        .Add(rolePermission);

    public Task<bool> ExistsAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken
    ) => app
        .RolesPermissions
        .AnyAsync(rp =>
            rp.RoleID == roleId
            && rp.PermissionID == permissionId,
            cancellationToken
        );

    public Task DeleteAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken
    ) => app
        .RolesPermissions
        .Where(rp =>
            rp.RoleID == roleId
            && rp.PermissionID == permissionId
        ).ExecuteDeleteAsync(cancellationToken);

    public Task<List<RolePermission>> ToListAsync(
        CancellationToken cancellationToken
    ) => app
        .RolesPermissions
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}