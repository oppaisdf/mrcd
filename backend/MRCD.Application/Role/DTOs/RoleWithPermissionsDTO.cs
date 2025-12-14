namespace MRCD.Application.Role.DTOs;

public sealed record RoleWithPermissionDTO(
    Guid RoleID,
    string RoleName,
    IEnumerable<PermissionUsedInRoleDTO> Permissions
);

public sealed record PermissionUsedInRoleDTO(
    Guid PermissionID,
    string PermissionName,
    bool IsUsed
);