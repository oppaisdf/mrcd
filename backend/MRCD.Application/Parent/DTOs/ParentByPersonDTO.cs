namespace MRCD.Application.Parent.DTOs;

public sealed record ParentByPersonDTO(
    Guid PersonId,
    string ParentName,
    bool IsParent,
    string? Phone
);
