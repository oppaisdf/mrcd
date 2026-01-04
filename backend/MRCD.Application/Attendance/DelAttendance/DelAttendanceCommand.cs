using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.DelAttendance;

public sealed record DelAttendanceCommand(
    Guid UserId,
    Guid PersonId,
    DateOnly Date
) : ICommand<Result>;