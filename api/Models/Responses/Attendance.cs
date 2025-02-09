namespace api.Models.Responses;

public class AttendanceResponse
{
    public required int Id { get; set; }
    public required string User { get; set; }
    public required string Person { get; set; }
    public required DateTime Date { get; set; }
}

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