namespace MRCD.Application.BaseEntity.Contracts;

public interface IBaseEntityRepository<TEntity> where TEntity : Domain.Common.BaseEntity
{
    void Add(TEntity newRecord);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken);
}