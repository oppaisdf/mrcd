using Microsoft.EntityFrameworkCore;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Infrastructure.Repositories;

internal sealed class BaseEntityRepository<TEntity>(
    Persistence.AppContext app
) : IBaseEntityRepository<TEntity> where TEntity : BaseEntity
{
    private Persistence.AppContext _app = app;

    public void Add(
        TEntity newRecord
    ) => _app.Set<TEntity>().Add(newRecord);

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => _app
        .Set<TEntity>()
        .Where(e => e.ID == id)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<List<TEntity>> ToListAsync(
        CancellationToken cancellationToken
    ) => _app
        .Set<TEntity>()
        .ToListAsync(cancellationToken);
}