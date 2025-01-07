namespace api.Models.Responses;

public class ChargeResponse
{
    public required short Id { get; set; }
    public required string Name { get; set; }
    public required decimal Total { get; set; }
    public required bool IsActive { get; set; }
    public ICollection<PersonChargeResponse>? People { get; set; }
}

public class PersonChargeResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required bool Gender { get; set; }
    public required bool Day { get; set; }
    public required bool IsActive { get; set; }
}