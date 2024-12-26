using System.ComponentModel.DataAnnotations;

namespace api.Models.Entities;

public class Degree
{
    public short? Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string Name { get; set; }
}