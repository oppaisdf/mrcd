using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Degree : INameEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public short? Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string Name { get; set; }
}