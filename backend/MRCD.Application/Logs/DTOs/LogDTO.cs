namespace MRCD.Application.Logs.DTOs;

public sealed record LogDTO(
    string Username,
    TimeSpan Time,
    string Message
);