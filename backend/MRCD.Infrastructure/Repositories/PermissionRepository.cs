using Microsoft.EntityFrameworkCore;
using MRCD.Application.Permission.Contracts;
using MRCD.Domain.Role;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PermissionRepository(
    Persistence.AppContext app
) : IPermissionRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        Permission permission
    ) => _app.Permissions.Add(permission);

    public Task<bool> AlreadyExistsAsync(
        string name,
        CancellationToken cancellationToken
    ) => _app
        .Permissions
        .AnyAsync(p =>
            p.Name == name,
            cancellationToken
        );

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Permissions
        .Where(p => p.ID == id)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<bool> IdExistsAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Permissions
        .AnyAsync(p => p.ID == id, cancellationToken);

    public Task<List<Permission>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Permissions
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}