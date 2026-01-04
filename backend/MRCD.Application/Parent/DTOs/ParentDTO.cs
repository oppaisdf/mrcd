namespace MRCD.Application.Parent.DTOs;

public sealed record ParentDTO(
    Guid Id,
    string Name,
    bool IsMasculine,
    string? Phone
);