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
        .ToListAsync(cancellationToken);

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

        if (isSunday.HasValue)
            query = query.Where(p => p.IsSunday == isSunday.Value);
        if (isMasculine.HasValue)
            query = query.Where(p => p.IsMasculine == isMasculine.Value);
        if (!string.IsNullOrWhiteSpace(normalizedName))
            query = query.Where(p => p.NormalizedName.Contains(normalizedName));
        
        var total = await query.CountAsync(cancellationToken);
        var skip = (page - 1) * size;
        var people = await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(size)
            .Select(p => new SimplePersonDTO(p.ID, p.Name))
            .ToListAsync(cancellationToken);
        return Pagination<SimplePersonDTO>.Create(people, total, page, size);
    }
}