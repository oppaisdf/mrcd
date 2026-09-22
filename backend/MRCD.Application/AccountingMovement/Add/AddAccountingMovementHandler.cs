using MRCD.Application.Logs;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.Add;

internal sealed class AddAccountingMovementHandler(
    IAccountingMovementRepository repo,
    IPersistenceContext save,
    AuditLog<AddAccountingMovementHandler> logs
) : ICommandHandler<AddAccountingMovementCommand, Guid>
{
    private readonly IAccountingMovementRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly AuditLog<AddAccountingMovementHandler> _logs = logs;

    public async Task<Result<Guid>> HandleAsync(
        AddAccountingMovementCommand command,
        CancellationToken cancellationToken
    )
    {
        var movement = Domain.AccountingMovement.AccountingMovement.Create(command.Description, command.Amount);
        if (!movement.IsSuccess)
            return Result<Guid>.Failure(movement.Error!);
        _repo.Add(movement.Value!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.Write(command.UserId, "Accounting movement {movement} has been added with ID {id}.", command.Description, movement.Value!.ID);
        return Result<Guid>.Success(movement.Value!.ID);
    }
}