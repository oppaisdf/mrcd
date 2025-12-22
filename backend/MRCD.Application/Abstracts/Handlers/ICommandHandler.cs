using MRCD.Domain.Common;

namespace MRCD.Application.Abstracts.Handlers;

public interface ICommandHandler<in TCommand> where TCommand : ICommand<Result>
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IBaseCommandHandler<in TCommand, TEntity>
    where TCommand : ICommand<Result>
    where TEntity : Domain.Common.BaseEntity
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse, TEntity>
    where TCommand : ICommand<TResponse>
    where TEntity : Domain.Common.BaseEntity
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}