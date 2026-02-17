namespace MRCD.API.DTOs;

public sealed record AttendanceRequest(
    Guid PersonId,
    bool IsAttendance,
    DateOnly? Date = null
);