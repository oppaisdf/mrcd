using Microsoft.EntityFrameworkCore;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Infrastructure.Repositories;

internal sealed class BaseEntityRepository<TEntity>(
    Persistence.AppContext app
) : IBaseEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    private Persistence.AppContext _app = app;

    public void Add(
        TEntity newRecord
    ) => _app
        .Set<TEntity>()
        .Add(newRecord);

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Set<TEntity>()
        .SingleOrDefaultAsync(e =>
            e.ID == id,
            cancellationToken
        );

    public void Remove(
        TEntity record
    ) => _app
        .Set<TEntity>()
        .Remove(record);

    public Task<List<TEntity>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Set<TEntity>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}