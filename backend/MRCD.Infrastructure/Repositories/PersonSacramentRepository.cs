using Microsoft.EntityFrameworkCore;
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