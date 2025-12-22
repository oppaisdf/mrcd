using MRCD.Domain.Common;

namespace MRCD.Application.Abstracts.Handlers;

public interface IQueryHandler<in TQuery, TResponse> where TResponse : IQuery<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

public interface IQueryHandler<TResponse>
{
    Task<Result<TResponse>> HandleAsync(CancellationToken cancellationToken);
}

public interface IBaseQueryHandler<TEntity, TResponse> where TEntity : Domain.Common.BaseEntity
{
    Task<Result<TResponse>> HandleAsync(CancellationToken cancellationToken);
}