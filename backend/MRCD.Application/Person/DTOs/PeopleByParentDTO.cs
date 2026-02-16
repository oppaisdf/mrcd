namespace MRCD.Application.Person.DTOs;

public sealed record PeopleByParentDTO(
    Guid ID,
    string Name,
    bool IsChild
);