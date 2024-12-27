namespace api.Models.Filters;

public class AttendanceFilter
{
    public required short Page { get; set; }
    public string? UserId { get; set; }
    public int? PersonId { get; set; }
}