using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonDocumentRepository
{
    void Add(PersonDocument personDocument);
    Task<PersonDocument?> GetAsync(Guid personId, Guid documentId, CancellationToken cancellationToken);
    void Remove(PersonDocument personDocument);
}