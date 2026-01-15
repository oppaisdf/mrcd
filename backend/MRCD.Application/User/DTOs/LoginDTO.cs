namespace MRCD.Application.User.DTOs;

public sealed record LoginDTO(
    Guid UserId,
    IEnumerable<string> Roles
);