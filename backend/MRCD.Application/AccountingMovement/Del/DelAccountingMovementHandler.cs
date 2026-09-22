using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.Del;

internal sealed class DelAccountingMovementHandler(
    IAccountingMovementRepository repo,
    AuditLog<DelAccountingMovementHandler> logs
) : ICommandHandler<DelAccountingMovementCommand>
{
    private readonly IAccountingMovementRepository _repo = repo;
    private readonly AuditLog<DelAccountingMovementHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelAccountingMovementCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await _repo.ExistsIdAsync(command.MovementId, cancellationToken);
        if (!exists)
            return Result.Failure("El movimiento contable no existe");
        await _repo.DeleteAsync(command.MovementId, cancellationToken);
        _logs.Write(command.UserId, "Accounting movement {moement} has been deleted.", command.MovementId);
        return Result.Success();
    }
}