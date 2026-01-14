namespace MRCD.API.DTOs;

public sealed record TokenDTO(
    string AccessToken,
    DateTimeOffset ExpiresAtUTC
);
