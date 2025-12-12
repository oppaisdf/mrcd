namespace MRCD.Domain.Person;

public sealed record PersonDocument(
    Guid PersonId,
    Guid DocumentId
);