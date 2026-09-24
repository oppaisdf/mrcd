using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Permission.Contracts;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Role.Get.WithPermissions;

internal sealed class GetRoleWithPermissionsHandler(
    IRoleRepository roleRepo,
    IPermissionRepository permissionRepo,
    IRolePermissionRepository rolePermissionRepo
) : IQueryHandler<IEnumerable<RoleWithPermissionDTO>>
{
    public async Task<Result<IEnumerable<RoleWithPermissionDTO>>> HandleAsync(
        CancellationToken cancellationToken
    )
    {
        var roles = await roleRepo.ToListAsync(cancellationToken);
        var rolePermission = await rolePermissionRepo.ToListAsync(cancellationToken);
        var rawPermissions = await permissionRepo.ToListAsync(cancellationToken);

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