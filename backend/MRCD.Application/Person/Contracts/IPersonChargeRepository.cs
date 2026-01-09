using MRCD.Application.BaseEntity.DTOs;
using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonChargeRepository
{
    void Add(PersonCharge personCharge);
    Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(Guid personId, CancellationToken cancellationToken);
    Task<PersonCharge?> GetAsync(Guid personId, Guid chargeId, CancellationToken cancellationToken);
    void Remove(PersonCharge personCharge);
}