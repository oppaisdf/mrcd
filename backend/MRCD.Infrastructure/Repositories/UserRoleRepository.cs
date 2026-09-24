using Microsoft.EntityFrameworkCore;
using MRCD.Application.User.Contracts;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Repositories;

internal sealed class UserRoleRepository(
    Persistence.AppContext app
) : IUserRoleRepository
{
    public void Add(
        UserRole userRole
    ) => app
        .UserRoles
        .Add(userRole);

    public void AddRange(
        IEnumerable<UserRole> userRoles
    ) => app
        .UserRoles
        .AddRange(userRoles);

    public Task DeleteAsync(
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken
    ) => app
        .UserRoles
        .Where(ur =>
            ur.UserID == userId
            && ur.RoleID == roleId
        )
        .ExecuteDeleteAsync(cancellationToken);

    public Task<List<UserRole>> RolesByUserIdToListAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => app
        .UserRoles
        .Where(ur => ur.UserID == id)
        .ToListAsync(cancellationToken);

    public Task<List<UserRole>> ToListAsync(
        CancellationToken cancellationToken
    ) => app
        .UserRoles
        .ToListAsync(cancellationToken);
}