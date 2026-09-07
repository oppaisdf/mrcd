using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Person.Get.GeneralList;

public sealed record GetGeneralListQuery(
    Guid UserId
) : IQuery<IEnumerable<GeneralListDTO>>;