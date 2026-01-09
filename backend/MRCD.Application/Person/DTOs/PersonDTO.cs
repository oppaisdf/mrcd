using MRCD.Application.BaseEntity.DTOs;
using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Person.DTOs;

public sealed record PersonDTO(
    string Name,
    bool IsActive,
    bool IsMasculine,
    bool IsSunday,
    DateOnly DOB,
    Guid DegreeId,
    string? Parish,
    string? Address,
    string? Phone,
    IEnumerable<ParentByPersonDTO> Parents,
    IEnumerable<ParentByPersonDTO> Godparents,
    IEnumerable<AssociationBaseEntityDTO> Charges,
    IEnumerable<AssociationBaseEntityDTO> Documents,
    IEnumerable<AssociationBaseEntityDTO> Sacraments
);