using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.AccountingMovement.Get;

public sealed record GetAccountingMovementQuery(
    Guid UserId,
    DateOnly Date,
    bool FilterOnlyByYear
) : IQuery<IEnumerable<Domain.AccountingMovement.AccountingMovement>>;