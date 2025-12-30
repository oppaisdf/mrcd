using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.GetAccountingMovement;

internal sealed class GetAccountingMovementHandler(
    IAccountingMovementRepository repo,
    ILogger<GetAccountingMovementHandler> logs
) : IQueryHandler<IEnumerable<Domain.AccountingMovement.AccountingMovement>, GetAccountingMovementQuery>
{
    private readonly IAccountingMovementRepository _repo = repo;
    private readonly ILogger<GetAccountingMovementHandler> _logs = logs;

    public Task<Result<IEnumerable<Domain.AccountingMovement.AccountingMovement>>> HandleAsync(
        GetAccountingMovementQuery query,
        CancellationToken cancellationToken
    )
    {
        var results = query.FilterOnlyByYear
            ? _repo.OnlyByYearToListAsync(query.Date.Year, cancellationToken)
            : _repo.ByDateToListAsync(query.Date, cancellationToken);
        _logs.LogInformation("Accounting movements listed by user {user} in date {date}", query.UserId, query.Date);
        return results
            .ContinueWith(r =>
                Result<IEnumerable<Domain.AccountingMovement.AccountingMovement>>.Success(r.Result),
                cancellationToken
            );
    }
}