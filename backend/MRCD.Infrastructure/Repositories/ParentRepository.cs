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

    public Task<List<ParentByPersonDTO>> ByPersonToListAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => (
        from p in _app.Parents
        join pp in _app.ParentsPersons on p.ID equals pp.ParentId
        where
            pp.PersonId == personId
        select new ParentByPersonDTO(
            pp.ParentId,
            p.ID,
            p.Name,
            pp.IsParent,
            p.IsMasculine,
            p.Phone
        )
    ).ToListAsync(cancellationToken);

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
            parent.ID,
            person.ID,
            parent.Name,
            pp.IsParent,
            parent.IsMasculine,
            parent.Phone
        )
    ).ToListAsync(cancellationToken);

    public Task<Parent?> GetByIdAsync(
        Guid parentId,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .AsNoTracking()
        .SingleOrDefaultAsync(p => p.ID == parentId, cancellationToken);

    public Task<Parent?> GetByNameAsync(
        string normalizedName,
        CancellationToken cancellationToken
    ) => _app
        .Parents
        .SingleOrDefaultAsync(p =>
            p.NormalizedName.Equals(normalizedName),
            cancellationToken
        );

    public async Task<Pagination<ParentDTO>> NoChildrenToListAsync(
        int page,
        int size,
        CancellationToken cancellationToken
    )
    {
        var query = _app
            .Parents
            .GroupJoin(
                _app.ParentsPersons,
                p => p.ID,
                pp => pp.ParentId,
                (p, pp) => new
                {
                    Parent = p,
                    HasChildren = pp.Any()
                }
            ).Where(g => !g.HasChildren)
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
                    p.Parent.ID,
                    p.Parent.Name,
                    p.Parent.IsMasculine,
                    p.Parent.Phone
                )),
            totalCount,
            page,
            size
        );
    }

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