using Microsoft.EntityFrameworkCore;
using MRCD.Application.BaseEntity.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Person;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonSacramentRepository(
    Persistence.AppContext app
) : IPersonSacramentRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        PersonSacrament personSacrament
    ) => _app
        .PersonSacraments
        .Add(personSacrament);

    public void AddRange(
        IEnumerable<PersonSacrament> sacraments
    ) => _app
        .PersonSacraments
        .AddRange(sacraments);

    public Task<List<AssociationBaseEntityDTO>> AssignationByPersonToListAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => (
        from s in _app.Sacraments
        join ps in _app
            .PersonSacraments
            .Where(p => p.PersonId == personId)
            on s.ID equals ps.SacramentId into pss
        select new AssociationBaseEntityDTO(
            s.ID,
            s.Name,
            pss.Any()
        )
    ).ToListAsync(cancellationToken);

    public Task<PersonSacrament?> GetAsync(
        Guid personId,
        Guid sacramentId,
        CancellationToken cancellationToken
    ) => _app
        .PersonSacraments
        .SingleOrDefaultAsync(ps =>
            ps.PersonId == personId
            && ps.SacramentId == sacramentId,
            cancellationToken
        );

    public void Remove(
        PersonSacrament personSacrament
    ) => _app
        .PersonSacraments
        .Remove(personSacrament);
}