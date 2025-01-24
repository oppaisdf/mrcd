using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Entities;

public class Attendance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [MaxLength(255)]
    public required string UserId { get; set; }
    public required int PersonId { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    [Required]
    [DefaultValue(true)]
    public required bool IsAttendance { get; set; }
}