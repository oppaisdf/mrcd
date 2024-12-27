using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class NameRequest
{
    [Required]
    [MaxLength(10, ErrorMessage = "Nombre demasiado largo")]
    public required string Name { get; set; }
}