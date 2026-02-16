using Microsoft.EntityFrameworkCore;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Parent;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ParentPersonRepository(
    Persistence.AppContext app
) : IParentPersonRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        ParentPerson parentPerson
    ) => _app
        .ParentsPersons
        .Add(parentPerson);

    public Task<int> AssignedCountAsync(
        Guid personId,
        bool isParent,
        CancellationToken cancellationToken
    ) => _app
        .ParentsPersons
        .Where(pp =>
            pp.IsParent == isParent
            && pp.PersonId == personId
        ).CountAsync(cancellationToken);

    public void Del(
        ParentPerson parentPerson
    ) => _app
        .ParentsPersons
        .Remove(parentPerson);

    public Task<ParentPerson?> GetAsync(
        Guid personId,
        Guid parentId,
        bool isParent,
        CancellationToken cancellationToken
    ) => _app
        .ParentsPersons
        .AsNoTracking()
        .Where(pp =>
            pp.ParentId == parentId
            && pp.PersonId == personId
            && pp.IsParent == isParent
        ).SingleOrDefaultAsync(cancellationToken);
}