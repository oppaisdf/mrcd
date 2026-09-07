using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.Del;

public sealed record DelAccountingMovementCommand(
    Guid UserId,
    Guid MovementId
) : ICommand<Result>;