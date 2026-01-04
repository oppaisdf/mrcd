using MRCD.Domain.User;

namespace MRCD.Application.User.Contracts;

public interface IUserRoleRepository
{
    void Add(UserRole userRole);
    void AddRange(IEnumerable<UserRole> userRoles);
    Task DeleteAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    Task<List<UserRole>> RolesByUserIdToListAsync(Guid id, CancellationToken cancellationToken);
    Task<List<UserRole>> ToListAsync(CancellationToken cancellationToken);
}