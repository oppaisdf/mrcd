namespace MRCD.Application.Alert.DTOs;

public sealed record AlertDTO(
    int Count,
    string Message
);