using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.AccountingMovement.Contracts;
using MRCD.Application.AccountingMovement.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.AccountingMovement.Get;

internal sealed class GetAccountingMovementHandler(
    IAccountingMovementRepository repo,
    AuditLog<GetAccountingMovementHandler> logs
) : IQueryHandler<IReadOnlyCollection<AccountingMovementDTO>, GetAccountingMovementQuery>
{
    private readonly IAccountingMovementRepository _repo = repo;
    private readonly AuditLog<GetAccountingMovementHandler> _logs = logs;

    public async Task<Result<IReadOnlyCollection<AccountingMovementDTO>>> HandleAsync(
        GetAccountingMovementQuery query,
        CancellationToken cancellationToken
    )
    {
        var rawMovements = query.FilterOnlyByYear
            ? await _repo.OnlyByYearToListAsync(query.Date.Year, cancellationToken)
            : await _repo.ByDateToListAsync(query.Date, cancellationToken);
        _logs.Write(query.UserId, "Accounting movement has been listed in date {date}", query.Date);

        var movements = rawMovements
            .Select(m => new
            {
                m.Amount,
                m.ID,
                m.Description,
                m.Date.Day,
                m.Date.Month
            }).GroupBy(m => m.Month)
            .Select(m => new AccountingMovementDTO(
                m.Key,
                [.. m.Select(rm => new SimpleAccountingMovementDTO(
                    rm.ID,
                    rm.Description,
                    rm.Amount,
                    rm.Day
                )).OrderBy(rm => rm.Day)]
            )).OrderBy(rm => rm.Month)
            .ToArray();

        return Result<IReadOnlyCollection<AccountingMovementDTO>>.Success(movements);
    }
}