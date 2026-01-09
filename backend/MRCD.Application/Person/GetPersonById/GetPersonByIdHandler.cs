using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.GetPersonById;

internal sealed class GetPersonByIdHandler(
    IPersonRepository person,
    IParentRepository parent,
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    ILogger<GetPersonByIdHandler> logs
) : IQueryHandler<PersonDTO, GetPersonByIdCommand>
{
    private readonly IPersonRepository _person = person;
    private readonly IParentRepository _parent = parent;
    private readonly IPersonChargeRepository _charge = charge;
    private readonly IPersonDocumentRepository _document = document;
    private readonly ILogger<GetPersonByIdHandler> _logs = logs;

    public async Task<Result<PersonDTO>> HandleAsync(
        GetPersonByIdCommand query,
        CancellationToken cancellationToken
    )
    {
        var person = await _person.GetByIdAsync(query.PersonId, cancellationToken);
        if (person is null)
            return Result<PersonDTO>.Failure("El confirmando no existe :c");

        var parentsTask = _parent.ByPersonToListAsync(query.PersonId, cancellationToken);
        var chargesTask = _charge.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var documentTask = _document.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var sacramentTask = _document.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        await Task.WhenAll(parentsTask, chargesTask, documentTask, sacramentTask);
        var response = new PersonDTO(
            person.Name,
            person.IsActive,
            person.IsMasculine,
            person.IsActive,
            person.DOB,
            person.LastDegreeId,
            person.Parish,
            person.Address,
            person.Phone,
            parentsTask.Result.Where(p => p.IsParent),
            parentsTask.Result.Where(p => !p.IsParent),
            chargesTask.Result,
            documentTask.Result,
            sacramentTask.Result
        );
        _logs.LogInformation("Person {person} has been consulted by user {user}", query.PersonId, query.UserId);
        return Result<PersonDTO>.Success(response);
    }
}