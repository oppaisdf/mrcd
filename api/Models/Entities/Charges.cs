using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Entities;

public class Charge
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public short? Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string Name { get; set; }

    [Required]
    [Precision(6, 2)]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public required decimal Total { get; set; }
}

public class PersonCharge
{
    public required int PersonId { get; set; }
    public required short ChargeId { get; set; }

    [Required]
    [Precision(6, 2)]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public required decimal Total { get; set; }
}