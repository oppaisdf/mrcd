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

    public Task<List<Charge>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Charges
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}