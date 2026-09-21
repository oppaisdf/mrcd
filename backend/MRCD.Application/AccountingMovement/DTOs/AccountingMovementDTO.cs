namespace MRCD.Application.AccountingMovement.DTOs;

public sealed record AccountingMovementDTO(
    int Month,
    SimpleAccountingMovementDTO[] Movements
);

public sealed record SimpleAccountingMovementDTO(
    Guid Id,
    string Description,
    decimal Amount,
    int Day
);