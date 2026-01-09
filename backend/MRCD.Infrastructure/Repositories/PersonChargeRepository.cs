using Microsoft.EntityFrameworkCore;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonChargeRepository(
    Persistence.AppContext app
) : IPersonChargeRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        PersonCharge personCharge
    ) => _app
        .PersonCharges
        .Add(personCharge);

    public Task<PersonCharge?> GetAsync(
        Guid personId,
        Guid chargeId,
        CancellationToken cancellationToken
    ) => _app
        .PersonCharges
        .SingleOrDefaultAsync(pc =>
            pc.PersonId == personId
            && pc.ChargeId == chargeId,
            cancellationToken
        );

    public void Remove(
        PersonCharge personCharge
    ) => _app
        .PersonCharges
        .Remove(personCharge);
}