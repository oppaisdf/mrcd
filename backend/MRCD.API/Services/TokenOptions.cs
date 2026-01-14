namespace MRCD.API.Services;

public sealed record TokenOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int LifetimeMinutes,
    int ClockskewSeconds
);