namespace api.Models.Responses;

public record AttendanceResponse(
    string Name,
    bool Day,
    bool Gender,
    IEnumerable<DateAttendanceResponse> Dates
);

public record DateAttendanceResponse(
    int Day,
    int Month,
    bool? HasAttendance
);

public class QRResponse
{
    public required string Name { get; set; }
    public required bool Day { get; set; }
    public required bool Gender { get; set; }
    public required string Hash { get; set; }
}

public class GeneralListResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
    public required bool Day { get; set; }
    public required DateTime DOB { get; set; }
    public string? Phone { get; set; }
    public IEnumerable<GeneralParentListResponse>? Parents { get; set; }
}

public class GeneralParentListResponse
{
    public required string Name { get; set; }
    public string? Phone { get; set; }
}