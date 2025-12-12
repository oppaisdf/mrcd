namespace MRCD.Domain.Person;

public sealed record PersonSacrament(
    Guid PersonId,
    Guid SacramentId
);