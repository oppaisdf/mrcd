namespace MRCD.Domain.User;

public sealed record UserRole(
    Guid RoleID,
    Guid UserID
);