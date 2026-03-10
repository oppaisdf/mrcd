namespace MRCD.Application.Logs.DTOs;

public sealed record LogDTO(
    string Username,
    DateTime Time,
    string Message
);