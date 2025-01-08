using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;
public class ParentRequest
{
    [MaxLength(50, ErrorMessage = "Nombre demasiado largo")]
    public string? Name { get; set; }

    [DefaultValue(true)]
    public bool? Gender { get; set; }
    public bool? IsParent { get; set; }

    [MaxLength(8, ErrorMessage = "Teléfono inválido")]
    [MinLength(8, ErrorMessage = "Teléfono inválido")]
    public string? Phone { get; set; }
}

public class ParentFilter
{
    public required short Page { get; set; }
    public bool? Gender { get; set; }
    public bool? IsParent { get; set; }
    public string? Name { get; set; }
}