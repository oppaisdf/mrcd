namespace MRCD.Application.Charge.Contracts;

public interface IChargeRepository
{
    void Add(Domain.Charge.Charge charge);
    Task<List<Domain.Charge.Charge>> ToListAsync(CancellationToken cancellationToken);
}