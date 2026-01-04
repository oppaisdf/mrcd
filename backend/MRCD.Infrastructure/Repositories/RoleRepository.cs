using Microsoft.EntityFrameworkCore;
using MRCD.Application.Role.Contracts;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Repositories;

internal sealed class RoleRepository(
    Persistence.AppContext app
) : IRoleRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        Role role
    ) => _app.Roles.Add(role);

    public Task<bool> AlreadyExistsAsync(
        string name,
        CancellationToken cancellationToken
    ) => _app
        .Roles
        .AnyAsync(r => r.Name == name, cancellationToken);

    public Task<List<Role>> ByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken
    ) => (
        from r in _app.Roles
        join ur in _app.UserRoles on r.ID equals ur.RoleID
        where
            ur.UserID == userId
        select r
    ).ToListAsync(cancellationToken);

    public Task<bool> IdExistsAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app.Roles
        .AnyAsync(r => r.ID == id, cancellationToken);

    public Task<List<Role>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Roles
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}