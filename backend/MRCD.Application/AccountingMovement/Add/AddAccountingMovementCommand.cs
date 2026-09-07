using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.AccountingMovement.Add;

public sealed record AddAccountingMovementCommand(
    Guid UserId,
    string Description,
    decimal Amount
) : ICommand<Guid>;