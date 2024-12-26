namespace api.Models.Responses;

public class PersonResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
    public required DateTime DOB { get; set; }
    public required bool Day { get; set; }
    public string? Parish { get; set; }
    public required short DegreeId { get; set; }
    public string? Address { get; set; }
    public required bool IsActive { get; set; }
    public ICollection<ParentResponse>? Parents { get; set; }
    public ICollection<ParentResponse>? Godparents { get; set; }
}

public class ParentResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
}