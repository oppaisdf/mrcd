using Microsoft.EntityFrameworkCore;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Parent;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ParentPersonRepository(
    Persistence.AppContext app
) : IParentPersonRepository
{
    public void Add(
        ParentPerson parentPerson
    ) => app
        .ParentsPersons
        .Add(parentPerson);

    public Task<int> AssignedCountAsync(
        Guid personId,
        bool isParent,
        CancellationToken cancellationToken
    ) => app
        .ParentsPersons
        .Where(pp =>
            pp.IsParent == isParent
            && pp.PersonId == personId
        ).CountAsync(cancellationToken);

    public void Del(
        ParentPerson parentPerson
    ) => app
        .ParentsPersons
        .Remove(parentPerson);

    public Task<ParentPerson?> GetAsync(
        Guid personId,
        Guid parentId,
        bool isParent,
        CancellationToken cancellationToken
    ) => app
        .ParentsPersons
        .AsNoTracking()
        .Where(pp =>
            pp.ParentId == parentId
            && pp.PersonId == personId
            && pp.IsParent == isParent
        ).SingleOrDefaultAsync(cancellationToken);
}