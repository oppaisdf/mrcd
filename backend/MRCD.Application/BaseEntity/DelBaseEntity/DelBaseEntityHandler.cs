using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.DelBaseEntity;

internal sealed class DelBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo,
    IPersistenceContext save,
    ILogger<DelBaseEntityHandler<TEntity>> logs
) : IBaseCommandHandler<DelBaseEntityCommand, TEntity>
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IBaseEntityRepository<TEntity> _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<DelBaseEntityHandler<TEntity>> _logs = logs;

    public async Task<Result> HandleAsync(
        DelBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var record = await _repo.GetByIdAsync(command.Id, cancellationToken);
        if (record is null)
            return Result.Failure("El registro no existe");
        _repo.Remove(record);
        _logs.LogInformation("Record {record} has been deleted by user {user}", command.Id, command.UserId);
        await _save.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}