namespace MRCD.Application.Attendance.DTOs;

public sealed record AttendanceDTO(
    string PersonName,
    IEnumerable<HasAttendanceDTO> Dates
);

public sealed record HasAttendanceDTO(
    DateOnly Date,
    AttendanceType Type
);

public enum AttendanceType
{
    Attended,
    Excused,
    Absent
}