namespace MRCD.Application.BaseEntity.DTOs;

public sealed record AssociationBaseEntityDTO(
    Guid ID,
    string Name,
    bool HasAssociation
);