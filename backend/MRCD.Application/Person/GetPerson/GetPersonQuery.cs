using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Common;
using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Person.GetPerson;

public sealed record GetPersonQuery(
    Guid UserId,
    bool IsActive,
    ushort Page,
    string? Name,
    bool? IsSunday,
    bool? IsMasculine,
    AlertType? Alert
) : IQuery<Pagination<SimplePersonDTO>>;