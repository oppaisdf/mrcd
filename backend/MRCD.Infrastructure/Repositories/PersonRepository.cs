using Microsoft.EntityFrameworkCore;
using MRCD.Application.Person.Contracts;
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
}