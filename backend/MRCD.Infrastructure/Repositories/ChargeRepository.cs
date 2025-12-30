using Microsoft.EntityFrameworkCore;
using MRCD.Application.Charge.Contracts;
using MRCD.Domain.Charge;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ChargeRepository(
    Persistence.AppContext app
) : IChargeRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        Charge charge
    ) => _app
        .Charges
        .Add(charge);

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Charges
        .Where(c => c.ID == id)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<bool> ExistsIdAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Charges
        .AnyAsync(c => c.ID == id, cancellationToken);

    public Task<List<Charge>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Charges
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}