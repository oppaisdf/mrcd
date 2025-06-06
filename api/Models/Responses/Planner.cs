using System.ComponentModel.DataAnnotations;

namespace api.Models.Responses;

public record SimplePlannerResponse(
    [Required]
    uint Id,
    [Required]
    string Name,
    [Required]
    DateTime Date
);

public record ActivityStageResponse(
    [Required]
    ushort StageId,
    [Required]
    string Name,
    [Required]
    bool MainUser,
    string? UserId,
    string? Notes
);

public record PlannerResponse(
    [Required]
    string Name,
    [Required]
    DateTime Date,
    ICollection<ActivityStageResponse> Activities
);