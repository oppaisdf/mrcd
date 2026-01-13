using MRCD.Domain.Common;

namespace MRCD.Application.Abstracts.Handlers;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

public interface IQueryHandler<TResponse>
{
    Task<Result<TResponse>> HandleAsync(CancellationToken cancellationToken);
}

public interface IBaseQueryHandler<TEntity> where TEntity : Domain.Common.BaseEntity
{
    Task<Result<IEnumerable<TEntity>>> HandleAsync(CancellationToken cancellationToken);
}