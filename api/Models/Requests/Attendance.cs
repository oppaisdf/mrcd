namespace api.Models.Requests;

public record AttendanceRequest(
    string Hash,
    bool? IsAttendance,
    DateTime? Date
);