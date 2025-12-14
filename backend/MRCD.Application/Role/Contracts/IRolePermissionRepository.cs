using MRCD.Domain.Role;

namespace MRCD.Application.Role.Contracts;

public interface IRolePermissionRepository
{
    void Add(RolePermission rolePermission);
    Task<bool> AlreadyExists(Guid roleId, Guid permissionId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken);
    Task<List<RolePermission>> ToListAsync(CancellationToken cancellationToken);
}