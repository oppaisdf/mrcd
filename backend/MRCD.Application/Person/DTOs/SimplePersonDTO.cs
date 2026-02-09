namespace MRCD.Application.Person.DTOs;

public sealed record SimplePersonDTO(
    Guid ID,
    string Name
);