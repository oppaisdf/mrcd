using System.ComponentModel.DataAnnotations;

namespace api.Models.Entities;

public class Attendance
{
    public int? Id { get; set; }

    [MaxLength(255)]
    public required string UserId { get; set; }
    public required int PersonId { get; set; }

    [Required]
    public required DateTime Date { get; set; }
}