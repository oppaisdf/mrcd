using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Document : INameEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public short? Id { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Name { get; set; }
}

public class PersonDocument
{
    public required int PersonId;
    public required short DocumentId;
}