using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Parent.DTOs;

public sealed record ParentDetailsDTO(
    string Name,
    bool IsMasculine,
    string? Phone,
    IEnumerable<SimplePersonDTO> Children,
    IEnumerable<SimplePersonDTO> Goodchildren
);