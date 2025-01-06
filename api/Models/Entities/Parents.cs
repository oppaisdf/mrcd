using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Parent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required string NameHash { get; set; }

    [Required]
    [DefaultValue(true)]
    public required bool Gender { get; set; }
    public string? Phone { get; set; }
}

public class ParentPerson
{
    public required int PersonId { get; set; }
    public required int ParentId { get; set; }

    [Required]
    [DefaultValue(true)]
    public required bool IsParent { get; set; }
}