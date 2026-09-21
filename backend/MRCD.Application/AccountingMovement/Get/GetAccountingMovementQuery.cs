using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.DTOs;

namespace MRCD.Application.AccountingMovement.Get;

public sealed record GetAccountingMovementQuery(
    Guid UserId,
    DateOnly Date,
    bool FilterOnlyByYear
) : IQuery<IReadOnlyCollection<AccountingMovementDTO>>;