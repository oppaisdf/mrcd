using MRCD.Domain.Person;

namespace MRCD.Application.Person.Contracts;

public interface IPersonChargeRepository
{
    void Add(PersonCharge personCharge);
    Task<PersonCharge?> GetAsync(Guid personId, Guid chargeId, CancellationToken cancellationToken);
    void Remove(PersonCharge personCharge);
}