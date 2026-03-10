using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Logs.Contracts;
using MRCD.Application.Logs.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Logs.GetLogs;

internal sealed class GetLogsHandler(
    ILogRepository repo,
    IUserRepository user
) : IQueryHandler<Pagination<LogDTO>, GetLogsQuery>
{
    private readonly ILogRepository _repo = repo;
    private readonly IUserRepository _user = user;

    public async Task<Result<Pagination<LogDTO>>> HandleAsync(
        GetLogsQuery query,
        CancellationToken cancellationToken
    ) {
        var rawLogs = await _repo.ToListAsync(
            query.Size > 100 ? (ushort)100 : query.Size,
            query.Page,
            cancellationToken
        );
        var usersDir = (await _user.ToListAsync(cancellationToken))
            .ToDictionary(k => k.ID.ToString(), k => k.Username);
        var logs = rawLogs with {
            Items = rawLogs.Items.Select(l => l with {Username = usersDir.TryGetValue(l.Username, out string username) ? username : "unknown"})
        };
        return Result<Pagination<LogDTO>>.Success(logs);
    }
}