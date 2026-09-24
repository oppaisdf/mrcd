using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Get.ById;

internal sealed class GetPersonByIdHandler(
    IPersonRepository personRepo,
    IParentRepository parent,
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    IPersonSacramentRepository sacrament,
    AuditLog<GetPersonByIdHandler> logs
) : IQueryHandler<PersonDTO, GetPersonByIdQuery>
{
    public async Task<Result<PersonDTO>> HandleAsync(
        GetPersonByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var person = await personRepo.GetByIdAsync(query.PersonId, cancellationToken);
        if (person is null)
            return Result<PersonDTO>.Failure("El confirmando no existe :c");

        var parents = await parent.ByPersonToListAsync(query.PersonId, cancellationToken);
        var charges = await charge.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var documents = await document.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
        var sacraments = await sacrament.AssignationByPersonToListAsync(query.PersonId, cancellationToken);
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
        logs.Write(query.UserId, "Person {person} with ID {id} has been consulted.", person.Name, person.ID);
        return Result<PersonDTO>.Success(response);
    }
}