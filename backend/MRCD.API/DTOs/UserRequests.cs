namespace MRCD.API.DTOs;

public sealed record AddUserRequest(
    string Username,
    string Password,
    IEnumerable<Guid> Roles
);

public sealed record UpdateUserRequest(
    string? Username = null,
    string? Password = null,
    bool? IsActive = null
);