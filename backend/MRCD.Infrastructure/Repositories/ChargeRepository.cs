using Microsoft.EntityFrameworkCore;
using MRCD.Application.Charge.Contracts;
using MRCD.Domain.Charge;

namespace MRCD.Infrastructure.Repositories;

internal sealed class ChargeRepository(
    Persistence.AppContext app
) : IChargeRepository
{
    public void Add(
        Charge charge
    ) => app
        .Charges
        .Add(charge);

    public Task<List<Charge>> ToListAsync(
        CancellationToken cancellationToken
    ) => app
        .Charges
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}