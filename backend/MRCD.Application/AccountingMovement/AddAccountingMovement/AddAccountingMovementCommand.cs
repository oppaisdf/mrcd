using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.AccountingMovement.AddAccountingMovement;

public sealed record AddAccountingMovementCommand(
    Guid UserId,
    string Description,
    decimal Amount
) : ICommand<Guid>;