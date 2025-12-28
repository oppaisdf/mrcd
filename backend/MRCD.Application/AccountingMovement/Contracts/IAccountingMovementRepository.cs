namespace MRCD.Application.AccountingMovement.Contracts;

public interface IAccountingMovementRepository
{
    void Add(Domain.AccountingMovement.AccountingMovement movement);

    /// <summary>
    /// Filtra registros por mes y año
    /// </summary>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Listado de movimientos contables</returns>
    Task<List<Domain.AccountingMovement.AccountingMovement>> ByDateToListAsync(DateOnly date, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}