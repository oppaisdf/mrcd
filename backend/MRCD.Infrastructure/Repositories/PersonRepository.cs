using Microsoft.EntityFrameworkCore;
using MRCD.Application.Person.Contracts;

namespace MRCD.Infrastructure.Repositories;

internal sealed class PersonRepository(
    Persistence.AppContext app
) : IPersonRepository
{
    private readonly Persistence.AppContext _app = app;

    public Task<bool> ExistsActiveAsync(
        Guid personId,
        CancellationToken cancellationToken
    ) => _app
        .People
        .AnyAsync(p =>
            p.ID == personId
            && p.IsActive,
            cancellationToken
        );
}