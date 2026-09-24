using Microsoft.EntityFrameworkCore;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Repositories;

internal sealed class RoleRepository(
    Persistence.AppContext app
) : IRoleRepository
{
    public void Add(
        Role role
    ) => app.Roles.Add(role);

    public Task<bool> AlreadyExistsAsync(
        string name,
        CancellationToken cancellationToken
    ) => app
        .Roles
        .AnyAsync(r => r.Name == name, cancellationToken);

    public Task<List<Role>> ByUserIdToListAsync(
        Guid userId,
        CancellationToken cancellationToken
    ) => (
        from r in app.Roles
        join ur in app.UserRoles on r.ID equals ur.RoleID
        where
            ur.UserID == userId
        select r
    ).ToListAsync(cancellationToken);

    public Task<Role?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken
    ) => app
        .Roles
        .SingleOrDefaultAsync(r => r.ID == roleId, cancellationToken);

    public Task<bool> IdExistsAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => app.Roles
        .AnyAsync(r => r.ID == id, cancellationToken);

    public Task<List<Role>> ToListAsync(
        CancellationToken cancellationToken
    ) => app
        .Roles
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}