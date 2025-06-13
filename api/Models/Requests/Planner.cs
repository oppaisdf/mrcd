using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class ActivityRequest
{
    [Required]
    [MaxLength(50, ErrorMessage = "Actividad demasiada larga")]
    public required string Name { get; set; }

    [Required]
    public required DateTime Date { get; set; }
};

public class StageRequest
{
    [Required]
    [MaxLength(50, ErrorMessage = "Etapa de actividad demasiada larga")]
    public required string Name { get; set; }
}

public class ActivityStageRequest
{
    [Required]
    public required uint ActivityId { get; set; }

    [Required]
    public required ushort StageId { get; set; }

    [Required]
    public required bool MainUser { get; set; }

    [MaxLength(255, ErrorMessage = "Usuario incorrecto")]
    public string? UserId { get; set; }

    [MaxLength(100, ErrorMessage = "Notas demasiado largas")]
    public string? Notes { get; set; }
}