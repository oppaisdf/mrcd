using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonSacramentRepository
{
    void Add(PersonSacrament personSacrament);
    Task<PersonSacrament?> GetAsync(Guid personId, Guid sacramentId, CancellationToken cancellationToken);
    void Remove(PersonSacrament personSacrament);
}