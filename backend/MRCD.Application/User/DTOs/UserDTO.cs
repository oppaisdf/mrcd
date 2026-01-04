using MRCD.Application.Role.DTOs;

namespace MRCD.Application.User.DTOs;

public sealed record UserDTO(
    Guid ID,
    string Username,
    bool IsActive,
    IEnumerable<UsingRoleDTO> Roles
);