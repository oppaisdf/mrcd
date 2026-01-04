using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Parent;

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
}