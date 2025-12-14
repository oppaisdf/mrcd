using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.GetRoleWithPermissions;

internal sealed class GetRoleWithPermissionsHandler(
    IRoleRepository role,
    IPermissionRepository permission,
    IRolePermissionRepository rolePermission
) : IQueryHandler<IEnumerable<RoleWithPermissionDTO>>
{
    private readonly IRoleRepository _role = role;
    private readonly IPermissionRepository _permmission = permission;
    private readonly IRolePermissionRepository _rolePermission = rolePermission;

    public async Task<Result<IEnumerable<RoleWithPermissionDTO>>> HandleAsync(
        CancellationToken cancellationToken
    )
    {
        var roles = await _role.ToListAsync(cancellationToken);
        var rolePermission = await _rolePermission.ToListAsync(cancellationToken);
        var rawPermissions = await _permmission.ToListAsync(cancellationToken);

        var permissionByRole = rolePermission
            .GroupBy(rp => rp.RoleID)
            .ToDictionary(
                g => g.Key,
                g => g
                    .Select(x => x.PermissionID)
                    .ToHashSet()
            );
        var results = roles
            .Select(r =>
            {
                permissionByRole.TryGetValue(r.ID, out var assigned);
                var permissions = rawPermissions
                    .Select(p => new PermissionUsedInRoleDTO(
                        p.ID,
                        p.Name,
                        assigned?.Contains(p.ID) ?? false
                    ));
                return new RoleWithPermissionDTO(
                    r.ID,
                    r.Name,
                    permissions
                );
            });

        return Result<IEnumerable<RoleWithPermissionDTO>>.Success(results);
    }
}