using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Log
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string UserId { get; set; }

    [Required]
    public required DateTime Date { get; set; }
    public required short ActionId { get; set; }

    [MaxLength(50)]
    public string? Details { get; set; }
}

public class ActionLog
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public short? Id { get; set; }
    [Required]
    [MaxLength(10)]
    public required string Name { get; set; }
}