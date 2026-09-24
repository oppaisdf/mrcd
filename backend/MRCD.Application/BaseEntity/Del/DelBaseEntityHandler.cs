using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Del;

internal sealed class DelBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo,
    IPersistenceContext save,
    AuditLog<DelBaseEntityHandler<TEntity>> logs
) : IBaseCommandHandler<DelBaseEntityCommand, TEntity>
    where TEntity : Domain.Common.BaseEntity
{
    public async Task<Result> HandleAsync(
        DelBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var record = await repo.GetByIdAsync(command.Id, cancellationToken);
        if (record is null)
            return Result.Failure("El registro no existe");
        repo.Remove(record);
        logs.Write(command.UserId, "Record {record} with ID {id} has been deleted.", record.Name, record.ID);
        await save.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}