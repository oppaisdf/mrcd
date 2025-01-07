namespace api.Models.Responses;

public class ChargeResponse
{
    public required short Id { get; set; }
    public required string Name { get; set; }
    public required decimal Total { get; set; }
    public required bool IsActive { get; set; }
}