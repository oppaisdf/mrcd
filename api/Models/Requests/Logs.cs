using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class ActionLogRequest
{
    [Required]
    [MaxLength(10, ErrorMessage = "Nombre muy largo")]
    public required string Name { get; set; }
}