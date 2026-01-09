using Microsoft.EntityFrameworkCore;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonDocumentRepository(
    Persistence.AppContext app
) : IPersonDocumentRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        PersonDocument personDocument
    ) => _app
        .PersonDocuments
        .Add(personDocument);

    public Task<PersonDocument?> GetAsync(
        Guid personId,
        Guid documentId,
        CancellationToken cancellationToken
    ) => _app
        .PersonDocuments
        .SingleOrDefaultAsync(pd =>
            pd.PersonId == personId
            && pd.DocumentId == documentId,
            cancellationToken
        );

    public void Remove(
        PersonDocument personDocument
    ) => _app
        .PersonDocuments
        .Remove(personDocument);
}