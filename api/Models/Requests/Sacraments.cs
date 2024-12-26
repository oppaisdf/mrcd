using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class SacramentRequest
{
    [Required]
    [MaxLength(10, ErrorMessage = "Nombre demasiado largo")]
    public required string Name { get; set; }
}