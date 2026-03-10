using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Logs.Contracts;
using MRCD.Application.Logs.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Logs.GetLogs;

internal sealed class GetLogsHandler(
    ILogRepository repo
) : IQueryHandler<Pagination<LogDTO>, GetLogsQuery>
{
    private readonly ILogRepository _repo = repo;

    public Task<Result<Pagination<LogDTO>>> HandleAsync(
        GetLogsQuery query,
        CancellationToken cancellationToken
    ) => _repo
        .ToListAsync(
            query.Size > 100 ? (ushort)100 : query.Size,
            query.Page,
            cancellationToken
        ).ContinueWith(r => Result<Pagination<LogDTO>>.Success(r.Result), cancellationToken);
}