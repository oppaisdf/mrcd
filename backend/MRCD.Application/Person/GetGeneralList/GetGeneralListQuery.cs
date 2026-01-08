using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Person.GetGeneralList;

public sealed record GetGeneralListQuery(
    Guid UserId
) : IQuery<IEnumerable<GeneralListDTO>>;