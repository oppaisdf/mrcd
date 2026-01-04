using Microsoft.EntityFrameworkCore;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Parent;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ParentRepository(
    Persistence.AppContext app
) : IParentRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        Parent parent
    ) => _app
        .Parents
        .Add(parent);

    public Task<bool> AlreadyExists(
        string normalizedParentName,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .AnyAsync(p => p.NormalizedName.Equals(normalizedParentName), cancellationToken);

    public Task DeleteAsync(
        Guid parentId,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .Where(p => p.ID == parentId)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<bool> ExistsAsync(
        Guid parentId,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .AnyAsync(p => p.ID == parentId, cancellationToken);
}