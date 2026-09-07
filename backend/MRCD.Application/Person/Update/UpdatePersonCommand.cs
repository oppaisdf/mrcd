using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Update;

public sealed record UpdatePersonCommand(
    Guid UserId,
    Guid PersonId,
    string? Name,
    DateOnly? DOB,
    bool? IsActive,
    bool? IsSunday,
    string? Parish,
    string? Address,
    string? Phone,
    Guid? LastDegreeId
) : ICommand<Result>;