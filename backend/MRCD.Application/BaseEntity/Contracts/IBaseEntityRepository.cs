namespace MRCD.Application.BaseEntity.Contracts;

public interface IBaseEntityRepository<TEntity> where TEntity : Domain.Common.BaseEntity
{
    void Add(TEntity newRecord);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Remove(TEntity record);
    Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken);
}