namespace MRCD.Domain.Person;

public sealed record PersonCharge(
    Guid PersonId,
    Guid ChargeId
);