using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Person.DTOs;

public sealed record GeneralListDTO(
    Guid PersonId,
    string PersonName,
    bool IsMasculine,
    bool IsSunday,
    DateOnly DOB,
    DateOnly RegistrationDate,
    string? Parish,
    string? Phone,
    IEnumerable<SimpleParentDTO> Parents,
    IEnumerable<SimpleParentDTO> Godparents
);