using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Logs.DTOs;

namespace MRCD.Application.Logs.GetLogs;

public sealed record GetLogsQuery(
    ushort Size,
    uint Page
) : IQuery<Pagination<LogDTO>>;