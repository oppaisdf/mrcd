using Microsoft.EntityFrameworkCore;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Repositories;

internal sealed class RolePermissionRepository(
    Persistence.AppContext app
) : IRolePermissionRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        RolePermission rolePermission
    ) => _app
        .RolesPermissions
        .Add(rolePermission);

    public Task<bool> ExistsAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken
    ) => _app
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
    ) => _app
        .RolesPermissions
        .Where(rp =>
            rp.RoleID == roleId
            && rp.PermissionID == permissionId
        ).ExecuteDeleteAsync(cancellationToken);

    public Task<List<RolePermission>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .RolesPermissions
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}