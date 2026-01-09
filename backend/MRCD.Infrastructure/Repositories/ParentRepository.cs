using Microsoft.EntityFrameworkCore;
using MRCD.Application.Common;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
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

    public Task<List<ParentByPersonDTO>> FilteredByActivePersonToListAsync(
        CancellationToken cancellationToken
    ) => (
        from person in _app.People
        join pp in _app.ParentsPersons on person.ID equals pp.PersonId
        join parent in _app.Parents on pp.ParentId equals parent.ID
        where
            person.IsActive
        select new ParentByPersonDTO(
            person.ID,
            parent.Name,
            pp.IsParent,
            parent.Phone
        )
    ).ToListAsync(cancellationToken);

    public Task<Parent?> GetByNameAsync(
        string normalizedName,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .SingleOrDefaultAsync(p =>
            p.NormalizedName.Equals(normalizedName),
            cancellationToken
        );

    public async Task<Pagination<ParentDTO>> ToListAsync(
        int page,
        int size,
        string? normalizedParentName,
        CancellationToken cancellationToken
    )
    {
        var query = _app
            .Parents
            .AsNoTracking()
            .Where(p =>
                normalizedParentName == null || p.NormalizedName.Contains(normalizedParentName)
            )
            .AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);
        var skip = (page - 1) * size;
        var parents = await query
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);
        return Pagination<ParentDTO>.Create(
            parents
                .Select(p => new ParentDTO(
                    p.ID,
                    p.Name,
                    p.IsMasculine,
                    p.Phone
                )),
            totalCount,
            page,
            size
        );
    }
}