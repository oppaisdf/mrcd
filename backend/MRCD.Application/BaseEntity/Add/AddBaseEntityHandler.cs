using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.BaseEntity.Services;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Add;

internal sealed class AddBaseEntityHandler<TEntity>(
    IBaseEntityRepository<TEntity> repo,
    NamedEntityPreparation<TEntity> preparation,
    IPersistenceContext save,
    AuditLog<AddBaseEntityHandler<TEntity>> logs
) : ICommandHandler<AddBaseEntityCommand, Guid, TEntity>
    where TEntity : Domain.Common.BaseEntity
{
    public async Task<Result<Guid>> HandleAsync(
        AddBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var newRecord = await preparation.PrepareAsync(command.Name, cancellationToken);
        if (!newRecord.IsSuccess)
            return Result<Guid>.Failure(newRecord.Error!);
        repo.Add(newRecord.Value!);
        await save.SaveChangesAsync(cancellationToken);
        logs.Write(command.UserId, "Record {record} with ID {id} has been created.", command.Name, newRecord.Value!.ID);
        return Result<Guid>.Success(newRecord.Value!.ID);
    }
}