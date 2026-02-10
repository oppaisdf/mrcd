namespace MRCD.API.DTOs;

public sealed record CreateParentRequest(
    string ParentName,
    bool IsMasculine,
    bool IsParent,
    string? Phone,
    Guid? PersonId
);

public sealed record AssignParentRequest(
    Guid ParentId,
    Guid PersonId,
    bool IsParent
);