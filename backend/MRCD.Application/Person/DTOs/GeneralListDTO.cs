using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Person.DTOs;

public sealed record GeneralListDTO(
    string PersonName,
    bool IsMasculine,
    bool IsSunday,
    DateOnly DOB,
    DateOnly RegistrationDate,
    string? Parish,
    string? Address,
    string? Phone,
    IEnumerable<ParentByPersonDTO> Parents,
    IEnumerable<ParentByPersonDTO> Godparents
);