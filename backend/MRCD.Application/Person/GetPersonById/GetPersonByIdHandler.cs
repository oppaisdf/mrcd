using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.GetPersonById;

internal sealed class GetPersonByIdHandler(
    IPersonRepository person,
    IParentRepository parent,
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    IPersonSacramentRepository sacrament,
    ILogger<GetPersonByIdHandler> logs
) : IQueryHandler<PersonDTO, GetPersonByIdQuery>
{
    private readonly IPersonRepository _person = person;
    private readonly IParentRepository _parent = parent;
    private readonly IPersonChargeRepository _charge = charge;
    private readonly IPersonDocumentRepository _document = document;
    private readonly IPersonSacramentRepository _sacrament = sacrament;
    private readonly ILogger<GetPersonByIdHandler> _logs = logs;

    public async Task<Result<PersonDTO>> HandleAsync(
        GetPersonByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var person = await _person.GetByIdAsync(query.PersonId, cancellationToken);
        if (person is null)
            return Result<PersonDTO>.Failure("El confirmando no existe :c");

        var parents = await _parent.ByPersonToListAsync(query.PersonId, cancellationToken);
        var charges = await _charge.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var documents = await _document.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var sacraments = await _sacrament.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
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
            parents.Where(p => p.IsParent).Select(p => new ParentDTO(p.ParentId, p.ParentName, p.IsMasculine, p.Phone)),
            parents.Where(p => !p.IsParent).Select(p => new ParentDTO(p.ParentId, p.ParentName, p.IsMasculine, p.Phone)),
            charges,
            documents,
            sacraments
        );
        _logs.LogInformation("Person {person} has been consulted by user {user}", query.PersonId, query.UserId);
        return Result<PersonDTO>.Success(response);
    }
}