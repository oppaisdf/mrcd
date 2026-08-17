using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using MRCD.Application.Common;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonRepository(
    Persistence.AppContext app
) : IPersonRepository
{
    private readonly Persistence.AppContext _app = app;

    private static async Task<Pagination<SimplePersonDTO>> GetPaginatedPeopleAsync(
        IQueryable<Person> query,
        int page,
        int size,
        string? normalizedName,
        bool? isSunday,
        bool? isMasculine,
        CancellationToken ct
    )
    {
        if (isSunday.HasValue)
            query = query.Where(p => p.IsSunday == isSunday.Value);
        if (isMasculine.HasValue)
            query = query.Where(p => p.IsMasculine == isMasculine.Value);
        if (!string.IsNullOrWhiteSpace(normalizedName))
            query = query.Where(p => p.NormalizedName.Contains(normalizedName));

        var total = await query.CountAsync(ct);
        var skip = (page - 1) * size;
        var people = await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(size)
            .Select(p => new SimplePersonDTO(p.ID, p.Name))
            .ToListAsync(ct);
        return Pagination<SimplePersonDTO>.Create(people, total, page, size);
    }

    public void Add(
        Person person
    ) => _app
        .People
        .Add(person);

    public Task<bool> AlreadyExistExceptIdAsync(
        string normalizedName,
        Guid personId,
        CancellationToken cancellationToken
    ) => _app
        .People
        .AnyAsync(p =>
            p.NormalizedName.Equals(normalizedName)
            && p.ID != personId,
            cancellationToken
        );

    public Task<bool> AlreadyExistsNameAsync(
        string normalizedName,
        CancellationToken cancellationToken
    ) => _app
        .People
        .AnyAsync(p =>
            p.NormalizedName.Equals(normalizedName),
            cancellationToken
        );

    public Task<List<PeopleByParentDTO>> ByPaerentToListAsync(
        Guid parentId,
        CancellationToken cancellationToken
    ) => (
        from p in _app.People
        join pp in _app.ParentsPersons on p.ID equals pp.PersonId
        where
            pp.ParentId == parentId
        orderby p.Name
        select new PeopleByParentDTO(
            p.ID,
            p.Name,
            pp.IsParent
        )
    ).ToListAsync(cancellationToken);

    public Task<bool> ExistsActiveAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => _app
        .People
        .AnyAsync(p =>
            p.ID == personId
            && p.IsActive,
            cancellationToken
        );

    public Task<Person?> GetByIdAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => _app
        .People
        .SingleOrDefaultAsync(p =>
            p.ID == personId,
            cancellationToken
        );

    public Task<List<Person>> OnlyActiveToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .People
        .AsNoTracking()
        .Where(p => p.IsActive)
        .OrderBy(p => p.Name)
        .ToListAsync(cancellationToken);

    public async Task<Pagination<SimplePersonDTO>> PendingChargesToListAsync(
        int page,
        int size,
        string? normalizedName,
        bool? isSunday,
        bool? isMasculine,
        CancellationToken cancellationToken
    )
    {
        var query = _app.People
            .AsNoTracking()
            .AsQueryable()
            .Where(p =>
                p.IsActive
                && _app.Charges.Any(c =>
                    !_app.PersonCharges.Any(pc =>
                        pc.PersonId == p.ID
                        && pc.ChargeId == c.ID
                    )
                )
            );
        return await GetPaginatedPeopleAsync(
            query,
            page,
            size,
            normalizedName,
            isSunday,
            isMasculine,
            cancellationToken
        );
    }

    public async Task<Pagination<SimplePersonDTO>> PendingDocumentsToListAsync(
        int page,
        int size,
        string? normalizedName,
        bool? isSunday,
        bool? isMasculine,
        CancellationToken cancellationToken
    )
    {
        var query = _app.People
            .AsNoTracking()
            .AsQueryable()
            .Where(p =>
                p.IsActive
                && _app.Documents.Any(d =>
                    !_app.PersonDocuments.Any(pd =>
                        pd.PersonId == p.ID
                        && pd.DocumentId == d.ID
                    )
                )
            );
        return await GetPaginatedPeopleAsync(
            query,
            page,
            size,
            normalizedName,
            isSunday,
            isMasculine,
            cancellationToken
        );
    }

    public async Task<Pagination<SimplePersonDTO>> PendingGodparentsToListAsync(
        int page,
        int size,
        string? normalizedName,
        bool? isSunday,
        bool? isMasculine,
        CancellationToken cancellationToken
    )
    {
        var query = _app.People
            .AsNoTracking()
            .AsQueryable()
            .Where(p =>
                p.IsActive
                && !_app.ParentsPersons.Any(pp =>
                    pp.ParentId == p.ID
                    && !pp.IsParent
                )
            );
        return await GetPaginatedPeopleAsync(
            query,
            page,
            size,
            normalizedName,
            isSunday,
            isMasculine,
            cancellationToken
        );
    }

    public async Task<Pagination<SimplePersonDTO>> ToListAsync(
        bool isActive,
        int page,
        int size,
        string? normalizedName,
        bool? isSunday,
        bool? isMasculine,
        CancellationToken cancellationToken
    )
    {
        var query = _app.People.AsNoTracking().AsQueryable();
        query = query.Where(p => p.IsActive == isActive);
        return await GetPaginatedPeopleAsync(
            query,
            page,
            size,
            normalizedName,
            isSunday,
            isMasculine,
            cancellationToken
        );
    }
}