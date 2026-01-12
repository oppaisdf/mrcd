using MRCD.Application.Person.AddPerson;

namespace MRCD.API.DTOs;

public sealed record AddPersonRequest(
    string Name,
    bool IsMasculine,
    bool IsSunday,
    DateOnly DOB,
    string Address,
    string? Phone,
    Guid DegreeId,
    IEnumerable<AddSimpleParentCommand> Parents
);

public sealed record UpdatePersonRequest(
    string? Name,
    DateOnly? DOB,
    bool? IsActive,
    bool? IsSunday,
    string? Parish,
    string? Address,
    string? Phone,
    Guid? DegreeId
);