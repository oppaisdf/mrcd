using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;
using MRCD.Domain.Degree;

namespace MRCD.Application.Person.AddPerson;

internal sealed class AddPersonHandler(
    IPersonRepository person,
    ICommonService service,
    IBaseEntityRepository<Degree> degree,
    IParentRepository parent,
    IParentPersonRepository parentPerson,
    IPersistenceContext save,
    ILogger<AddPersonHandler> logs
) : ICommandHandler<AddPersonCommand, Guid>
{
    private readonly IPersonRepository _person = person;
    private readonly ICommonService _service = service;
    private readonly IBaseEntityRepository<Degree> _degree = degree;
    private readonly IParentRepository _parent = parent;
    private readonly IParentPersonRepository _parentPerson = parentPerson;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddPersonHandler> _logs = logs;

    private async Task<Result> AddParentsAsync(
        Guid personId,
        IEnumerable<AddSimpleParentCommand> rawParents,
        CancellationToken ct
    )
    {
        var parents = rawParents.Distinct().ToList();
        if (parents.Count > 2)
            return Result.Failure("No puede agregar más de dos padres");
        foreach (var parent in parents)
        {
            var normalizedName = _service.NormalizeString(parent.Name);
            if (!_service.HasOnlyLetters(normalizedName))
                return Result.Failure($"El nombre del padre '{parent.Name}' solo debe contener letras");
            var existingParent = await _parent.GetByNameAsync(normalizedName, ct);
            if (existingParent is not null)
            {
                _parentPerson.Add(new(
                    existingParent.ID,
                    personId,
                    true
                ));
                continue;
            }
            var phone = string.IsNullOrWhiteSpace(parent.Phone)
                ? null : parent.Phone.Trim();
            if (phone is not null && !_service.HasOnlyNumbers(phone))
                return Result.Failure($"El teléfono {phone} es inválido para el padre {parent.Name}");
            var parentResult = Domain.Parent.Parent.Create(
                parent.Name,
                normalizedName,
                parent.IsMasculine,
                phone
            );
            if (!parentResult.IsSuccess)
                return Result.Failure(parentResult.Error!);
            _parent.Add(parentResult.Value!);
            _parentPerson.Add(new(
                parentResult.Value!.ID,
                personId,
                true
            ));
        }
        return Result.Success();
    }

    public async Task<Result<Guid>> HandleAsync(
        AddPersonCommand command,
        CancellationToken cancellationToken
    )
    {
        var phone = string.IsNullOrWhiteSpace(command.Phone)
            ? null : command.Phone.Trim();
        if (phone is not null && !_service.HasOnlyNumbers(phone))
            return Result<Guid>.Failure("El número telefónico no es válido");
        var normalizedName = _service.NormalizeString(command.Name);
        if (!_service.HasOnlyLetters(normalizedName))
            return Result<Guid>.Failure("El nombre del confirmando solo debe contener letras");
        var personResult = Domain.Person.Person.Create(
            command.Name,
            normalizedName,
            command.IsMasculine,
            command.IsSunday,
            command.DOB,
            command.DegreeId,
            phone,
            command.Address
        );
        if (!personResult.IsSuccess)
            return Result<Guid>.Failure(personResult.Error!);

        var alreadyExists = await _person.AlreadyExistsNameAsync(normalizedName, cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Failure("El confirmando ya se ha registrado");
        var degreeExists = await _degree.GetByIdAsync(command.DegreeId, cancellationToken) is not null;
        if (!degreeExists)
            return Result<Guid>.Failure("El grado académico no existe");

        var person = personResult.Value!;
        _person.Add(person);
        var parentsResult = await AddParentsAsync(person.ID, command.Parents, cancellationToken);
        if (!parentsResult.IsSuccess)
            return Result<Guid>.Failure(parentsResult.Error!);
        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("Person {person} has been added by user {user}", person.ID, command.UserId);
        return Result<Guid>.Success(personResult.Value!.ID);
    }
}