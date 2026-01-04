using Microsoft.EntityFrameworkCore;
using MRCD.Application.User.Contracts;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Repositories;

internal sealed class UserRoleRepository(
    Persistence.AppContext app
) : IUserRoleRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        UserRole userRole
    ) => _app
        .UserRoles
        .Add(userRole);

    public void AddRange(
        IEnumerable<UserRole> userRoles
    ) => _app
        .UserRoles
        .AddRange(userRoles);

    public Task DeleteAsync(
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken
    ) => _app
        .UserRoles
        .Where(ur =>
            ur.UserID == userId
            && ur.RoleID == roleId
        )
        .ExecuteDeleteAsync(cancellationToken);

    public Task<List<UserRole>> RolesByUserIdToListAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .UserRoles
        .Where(ur => ur.UserID == id)
        .ToListAsync(cancellationToken);

    public Task<List<UserRole>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .UserRoles
        .ToListAsync(cancellationToken);
}