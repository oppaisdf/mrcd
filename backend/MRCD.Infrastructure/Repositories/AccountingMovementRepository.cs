using Microsoft.EntityFrameworkCore;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.AccountingMovement;

namespace MRCD.Infrastructure.Repositories;

internal sealed class AccountingMovementRepository(
    Persistence.AppContext app
) : IAccountingMovementRepository
{
    private readonly Persistence.AppContext _app = app;

    public void Add(
        AccountingMovement movement
    ) => _app
        .AccountingMovements
        .Add(movement);

    public Task<List<AccountingMovement>> ByDateToListAsync(
        DateOnly date,
        CancellationToken cancellationToken
    ) => _app
        .AccountingMovements
        .AsNoTracking()
        .Where(m => m.Date.Year == date.Year && m.Date.Month == date.Month)
        .ToListAsync(cancellationToken);

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .AccountingMovements
        .Where(m => m.ID == id)
        .ExecuteDeleteAsync(cancellationToken);
}