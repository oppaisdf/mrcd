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
    private readonly IBaseEntityRepository<TEntity> _repo = repo;
    private readonly NamedEntityPreparation<TEntity> _preparation = preparation;
    private readonly IPersistenceContext _save = save;
    private readonly AuditLog<AddBaseEntityHandler<TEntity>> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddBaseEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var newRecord = await _preparation.PrepareAsync(command.Name, cancellationToken);
        if (!newRecord.IsSuccess)
            return Result<Guid>.Failure(newRecord.Error!);
        _repo.Add(newRecord.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.Write(command.UserId, "Record {record} with ID {id} has been created.", command.Name, newRecord.Value!.ID);
        return Result<Guid>.Success(newRecord.Value!.ID);
    }
}