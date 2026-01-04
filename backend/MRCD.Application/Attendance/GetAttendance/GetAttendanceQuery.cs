using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.DTOs;

namespace MRCD.Application.Attendance.GetAttendance;

public sealed record GetAttendanceQuery(
    DateOnly Date,
    bool FilteredOnlyByYear,
    bool? IsSunday,
    bool? IsMasculine,
    string? PersonName
) : IQuery<IEnumerable<AttendanceDTO>>;