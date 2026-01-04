using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.AddAttendance;

public sealed record AddAttendanceCommand(
    Guid UserId,
    Guid PersonId,
    bool IsAttendance
) : ICommand<Result>;