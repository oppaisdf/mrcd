using MRCD.Domain.User;

namespace MRCD.Application.User.Contracts;

public interface IUserRoleRepository
{
    void AddRange(IEnumerable<UserRole> userRoles);
    Task<List<UserRole>> RolesByUserIdToListAsync(Guid id, CancellationToken cancellationToken);
    Task<List<UserRole>> ToListAsync(CancellationToken cancellationToken);
}