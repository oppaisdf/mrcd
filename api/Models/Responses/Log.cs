namespace api.Models.Responses;

public class LogResponse
{
    public required string User { get; set; }
    public required DateTime Date { get; set; }
    public required string Action { get; set; }
    public string? Details { get; set; }
}