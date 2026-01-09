using MRCD.Application.BaseEntity.DTOs;
using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonDocumentRepository
{
    void Add(PersonDocument personDocument);
    Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(Guid personId, CancellationToken cancellationToken);
    Task<PersonDocument?> GetAsync(Guid personId, Guid documentId, CancellationToken cancellationToken);
    void Remove(PersonDocument personDocument);
}