namespace MRCD.Application.Charge.Contracts;

public interface IChargeRepository
{
    void Add(Domain.Charge.Charge charge);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Domain.Charge.Charge>> ToListAsync(CancellationToken cancellationToken);
}