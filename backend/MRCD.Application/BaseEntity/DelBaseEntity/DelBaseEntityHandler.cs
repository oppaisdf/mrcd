using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.DelBaseEntity;

internal sealed class DelBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo,
    ILogger<DelBaseEntityHandler<TEntity>> logs
) : IBaseCommandHandler<DelBaseEntityCommand, TEntity>
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IBaseEntityRepository<TEntity> _repo = repo;
    private readonly ILogger<DelBaseEntityHandler<TEntity>> _logs = logs;

    public async Task<Result> HandleAsync(
        DelBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsIdAsync(command.Id, cancellationToken);
        if (!exists)
            return Result.Failure("El registro no existe");
        await _repo.DeleteAsync(command.Id, cancellationToken);
        _logs.LogInformation("Record {record} has been deleted by user {user}", command.Id, command.UserId);
        return Result.Success();
    }
}