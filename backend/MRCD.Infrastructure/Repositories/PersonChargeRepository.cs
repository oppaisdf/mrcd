using Microsoft.EntityFrameworkCore;
using MRCD.Application.BaseEntity.DTOs;
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

    public Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => (
        from c in _app.Charges
        join pc in _app
            .PersonCharges
            .Where(p => p.PersonId == personId)
            on c.ID equals pc.ChargeId into pcs
        select new AssociationBaseEntityDTO(
            c.ID,
            c.Name,
            pcs.Any()
        )
    ).ToListAsync(cancellationToken);

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