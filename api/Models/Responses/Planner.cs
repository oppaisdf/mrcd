using System.ComponentModel.DataAnnotations;

namespace api.Models.Responses;

public record DayResponse(
    [Required]
    ushort Day,
    [Required]
    IEnumerable<SimpleActivityResponse> Activities
);

public record SimpleActivityResponse(
    [Required]
    uint Id,
    [Required]
    string Name
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

public record StageResponse(
    [Required]
    ushort Id,
    [Required]
    string Name
);