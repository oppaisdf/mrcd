using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Person.GetPersonById;

public sealed record GetPersonByIdQuery(
    Guid UserId,
    Guid PersonId
) : IQuery<PersonDTO>;