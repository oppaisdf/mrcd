namespace api.Models.Responses;

public class LogResponse
{
    public required string User { get; set; }
    public required DateTime Date { get; set; }
    public required string Action { get; set; }
    public string? Details { get; set; }
}

public class FilterResponse
{
    public required ICollection<UserFilterResponse> Users { get; set; }
    public required ICollection<ActionFilterResponse> Actions { get; set; }
}

public class UserFilterResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}

public class ActionFilterResponse
{
    public required short Id { get; set; }
    public required string Name { get; set; }
}