using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.GetBaseEntity;

internal sealed class GetBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo
) : IBaseQueryHandler<TEntity, IEnumerable<TEntity>>
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IBaseEntityRepository<TEntity> _repo = repo;

    public Task<Result<IEnumerable<TEntity>>> HandleAsync(
        CancellationToken cancellationToken
    ) => _repo
        .ToListAsync(cancellationToken)
        .ContinueWith(r =>
            Result<IEnumerable<TEntity>>.Success(r.Result),
            cancellationToken
        );
}