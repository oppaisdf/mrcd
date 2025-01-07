using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class ChargeRequest
{
    [MaxLength(10)]
    public string? Name { get; set; }
    public decimal? Total { get; set; }
}