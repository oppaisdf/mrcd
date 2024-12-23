namespace api.Models.Responses;

public class UserResponse
{
    public required string Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public required List<string> Roles { get; set; }
}