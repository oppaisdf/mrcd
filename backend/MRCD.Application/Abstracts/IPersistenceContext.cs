namespace MRCD.Application.Abstracts;

public interface IPersistenceContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}