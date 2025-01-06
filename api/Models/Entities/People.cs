using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Person
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required]
    [MaxLength(64)]
    public required string Hash { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    [DefaultValue(true)]
    public required bool IsActive { get; set; }

    [Required]
    [DefaultValue(true)]
    public required bool Gender { get; set; }

    [Required]
    [DefaultValue(false)]
    public required bool Day { get; set; }

    [Required]
    public required DateTime DOB { get; set; }

    [MaxLength(30)]
    public string? Parish { get; set; }
    public required short DegreeId { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(8)]
    [MinLength(8)]
    public string? Phone { get; set; }
}
