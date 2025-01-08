namespace api.Models.Responses;

public class ParentResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
    public required bool IsParent { get; set; }
    public string? Phone { get; set; }
    public ICollection<BasicPersonResponse>? People { get; set; }
}