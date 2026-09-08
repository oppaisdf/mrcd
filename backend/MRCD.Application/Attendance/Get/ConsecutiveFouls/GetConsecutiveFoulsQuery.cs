using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.DTOs;

namespace MRCD.Application.Attendance.Get.ConsecutiveFouls;

public sealed record GetConsecutiveFoulsQuery(
    ushort ConsecutiveWeeksCount
) : IQuery<IReadOnlyCollection<AttendanceDTO>>;