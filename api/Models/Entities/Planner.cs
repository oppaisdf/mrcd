using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Activity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint? Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required DateTime Date { get; set; }
}

public class ActivityStage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort? Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
}

public class StagesOfActivities
{
    public required uint ActivityId { get; set; }
    public required ushort StageId { get; set; }

    [MaxLength(255)]
    public string? UserId { get; set; }

    [Required]
    [DefaultValue(false)]
    public required bool MainUser { get; set; }
}
