namespace MRCD.Domain.Role;

public sealed record RolePermission(
    Guid RoleID,
    Guid PermissionID
);