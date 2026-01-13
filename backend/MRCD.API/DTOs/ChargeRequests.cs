namespace MRCD.API.DTOs;

public sealed record AddChargeRequest(
    string Name,
    decimal Amount
);