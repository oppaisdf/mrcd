using Microsoft.EntityFrameworkCore;
using MRCD.Application.Common;
using MRCD.Application.Logs.Contracts;
using MRCD.Application.Logs.DTOs;

namespace MRCD.Infrastructure.Repositories;

internal sealed class LogRepository(
    Persistence.AppContext app
) : ILogRepository
{
    private readonly Persistence.AppContext _app = app;

    public async Task<Pagination<LogDTO>> ToListAsync(
        ushort size,
        uint page,
        CancellationToken cancellationToken
    )
    {
        var skip = (page - 1) * size;
        var total = await _app
            .Database
            .SqlQuery<int>($"select count(1) from logs;")
            .SingleAsync(cancellationToken);
        var logs = await _app
            .Database
            .SqlQuery<LogDTO>($"""
            select
                Properties ->>'$.UserId' as Username,
                _ts as Time,
                Message
            from
                logs
            where
                Properties ->> '$.UserId' != 'null'
            order by
                _ts desc
            limit {size} offset {skip};
            """)
            .ToListAsync(cancellationToken);
        return Pagination<LogDTO>.Create(
            logs,
            total,
            (int)page,
            size
        );
    }
}