using Microsoft.EntityFrameworkCore;
using MRCD.Application.BaseEntity.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonDocumentRepository(
    Persistence.AppContext app
) : IPersonDocumentRepository
{
    public void Add(
        PersonDocument personDocument
    ) => app
        .PersonDocuments
        .Add(personDocument);

    public Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => (
        from d in app.Documents
        join dp in app
            .PersonDocuments
            .Where(p => p.PersonId == personId)
            on d.ID equals dp.DocumentId into pds
        select new AssociationBaseEntityDTO(
            d.ID,
            d.Name,
            pds.Any()
        )
    ).ToListAsync(cancellationToken);

    public Task<PersonDocument?> GetAsync(
        Guid personId,
        Guid documentId,
        CancellationToken cancellationToken
    ) => app
        .PersonDocuments
        .SingleOrDefaultAsync(pd =>
            pd.PersonId == personId
            && pd.DocumentId == documentId,
            cancellationToken
        );

    public void Remove(
        PersonDocument personDocument
    ) => app
        .PersonDocuments
        .Remove(personDocument);
}