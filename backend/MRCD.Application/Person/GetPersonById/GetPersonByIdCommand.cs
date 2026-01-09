using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.DTOs;

namespace MRCD.Application.Person.GetPersonById;

public sealed record GetPersonByIdCommand(
    Guid UserId,
    Guid PersonId
) : IQuery<PersonDTO>;