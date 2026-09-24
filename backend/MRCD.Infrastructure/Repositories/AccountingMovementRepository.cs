using Microsoft.EntityFrameworkCore;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.AccountingMovement;

namespace MRCD.Infrastructure.Repositories;

internal sealed class AccountingMovementRepository(
    Persistence.AppContext app
) : IAccountingMovementRepository
{
    public void Add(
        AccountingMovement movement
    ) => app
        .AccountingMovements
        .Add(movement);

    public Task<List<AccountingMovement>> ByDateToListAsync(
        DateOnly date,
        CancellationToken cancellationToken
    ) => app
        .AccountingMovements
        .AsNoTracking()
        .Where(m => m.Date.Year == date.Year && m.Date.Month == date.Month)
        .ToListAsync(cancellationToken);

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => app
        .AccountingMovements
        .Where(m => m.ID == id)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<bool> ExistsIdAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => app
        .AccountingMovements
        .AnyAsync(m => m.ID == id, cancellationToken);

    public Task<List<AccountingMovement>> OnlyByYearToListAsync(
        int year,
        CancellationToken cancellationToken
    ) => app
        .AccountingMovements
        .Where(m => m.Date.Year == year)
        .ToListAsync(cancellationToken);
}