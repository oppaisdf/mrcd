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
    public string? Phone { get; set; }
    public required bool IsActive { get; set; }
    public ICollection<ParentResponse>? Parents { get; set; }
    public ICollection<ParentResponse>? Godparents { get; set; }
    public ICollection<DefaultEntityStatusResponse>? Sacraments { get; set; }
    public ICollection<DefaultEntityResponse>? Degrees { get; set; }
    public ICollection<ChargeResponse>? Charges { get; set; }
    public ICollection<DefaultEntityStatusResponse>? Documents { get; set; }
}

public class BasicPersonResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
    public required bool Day { get; set; }
    public required bool IsActive { get; set; }
    public bool? HasParent { get; set; }
}

public record DefaultEntityStatusResponse(
    short Id,
    string Name,
    bool IsActive
);

public class PersonFilterResponse
{
    public required ICollection<DefaultEntityResponse> Degrees { get; set; }
    public required ICollection<DefaultEntityResponse> Sacraments { get; set; }
    public decimal? Price { get; set; }
}

public class DefaultEntityResponse
{
    public required short Id { get; set; }
    public required string Name { get; set; }
}