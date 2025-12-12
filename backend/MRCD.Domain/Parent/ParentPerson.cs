namespace MRCD.Domain.Parent;

public sealed record ParentPerson(
    Guid ParentId,
    Guid PersonId,
    bool IsParent
);