using MRCD.Domain.Role;

namespace MRCD.Application.Role.Contracts;

public interface IRolePermissionRepository
{
    void Add(RolePermission rolePermission);
    Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken);
    Task<List<RolePermission>> ToListAsync(CancellationToken cancellationToken);
}