namespace api.Models.Filters;

public class LogFilter
{
    public required short Page { get; set; }
    public string? UserId { get; set; }
    public short? Action { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}