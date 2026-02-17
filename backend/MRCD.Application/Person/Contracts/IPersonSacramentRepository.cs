using MRCD.Application.BaseEntity.DTOs;
using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonSacramentRepository
{
    void Add(PersonSacrament personSacrament);
    void AddRange(IEnumerable<PersonSacrament> sacraments);
    Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(Guid personId, CancellationToken cancellationToken);
    Task<PersonSacrament?> GetAsync(Guid personId, Guid sacramentId, CancellationToken cancellationToken);
    void Remove(PersonSacrament personSacrament);
}