namespace MRCD.Application.Parent.DTOs;

public sealed record ParentByPersonDTO(
    Guid PersonId,
    Guid ParentId,
    string ParentName,
    bool IsParent,
    bool IsMasculine,
    string? Phone
);
