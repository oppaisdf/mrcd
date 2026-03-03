using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.AddAccountingMovement;

internal sealed class AddAccountingMovementHandler(
    IAccountingMovementRepository repo,
    IPersistenceContext save,
    ILogger<AddAccountingMovementHandler> logs
) : ICommandHandler<AddAccountingMovementCommand, Guid>
{
    private readonly IAccountingMovementRepository _repo = repo;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddAccountingMovementHandler> _logs = logs;

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
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = command.UserId
        }))
        {
            _logs.LogInformation("Accounting movement {movement} has been added with ID {id}.", command.Description, movement.Value!.ID);
        }
        return Result<Guid>.Success(movement.Value!.ID);
    }
}