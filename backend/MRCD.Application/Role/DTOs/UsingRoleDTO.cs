namespace MRCD.Application.Role.DTOs;

public sealed record UsingRoleDTO(
    Guid Id,
    string RoleName,
    bool HasRole
);