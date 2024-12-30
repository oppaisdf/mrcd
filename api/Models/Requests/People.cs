using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class PeopleRequest
{
    [MaxLength(50, ErrorMessage = "Nombre demasiado largo")]
    public string? Name { get; set; }

    [DefaultValue(true)]
    public bool? Gender { get; set; }

    public DateTime? DOB { get; set; }
    public bool? Day { get; set; }

    [MaxLength(30, ErrorMessage = "Parroquia demasiado grande")]
    public string? Parish { get; set; }
    public short? DegreeId { get; set; }

    [MaxLength(100, ErrorMessage = "Dirección demasiado grande")]
    public string? Address { get; set; }

    [MaxLength(8, ErrorMessage = "Número de teléfono inválido")]
    [MinLength(8, ErrorMessage = "Número de teléfono inválido")]
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<ParentRequest>? Parents { get; set; }
    public ICollection<ParentRequest>? Godparents { get; set; }
    public ICollection<short>? Sacraments { get; set; }
}

public class ParentRequest
{
    [MaxLength(50, ErrorMessage = "Nombre demasiado largo")]
    public required string Name { get; set; }

    [DefaultValue(true)]
    public required bool Gender { get; set; }
}