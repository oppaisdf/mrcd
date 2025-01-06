using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;
public class ParentRequest
{
    [MaxLength(50, ErrorMessage = "Nombre demasiado largo")]
    public string? Name { get; set; }

    [DefaultValue(true)]
    public bool? Gender { get; set; }

    [MaxLength(8, ErrorMessage = "Teléfono inválido")]
    [MinLength(8, ErrorMessage = "Teléfono inválido")]
    public string? Phone { get; set; }
}

public class AssignParentRequest
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required bool IsParent { get; set; }
}