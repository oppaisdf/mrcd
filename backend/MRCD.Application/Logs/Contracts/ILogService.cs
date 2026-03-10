using MRCD.Application.Common;
using MRCD.Application.Logs.DTOs;

namespace MRCD.Application.Logs.Contracts;

public interface ILogRepository
{
    Task<Pagination<LogDTO>> ToListAsync(
        ushort size,
        uint page,
        CancellationToken cancellationToken
    );
}