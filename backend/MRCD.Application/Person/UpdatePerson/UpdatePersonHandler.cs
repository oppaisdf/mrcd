using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;
using MRCD.Domain.Degree;

namespace MRCD.Application.Person.UpdatePerson;

internal sealed class UpdatePersonHandler(
    IPersonRepository person,
    ICommonService service,
    IBaseEntityRepository<Degree> degree,
    IPersistenceContext save,
    ILogger<UpdatePersonHandler> logs
) : ICommandHandler<UpdatePersonCommand>
{
    private readonly IPersonRepository _person = person;
    private readonly ICommonService _service = service;
    private readonly IBaseEntityRepository<Degree> _degree = degree;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<UpdatePersonHandler> _logs = logs;

    private enum PropertyType
    {
        Address,
        Parish,
        Phone,
        IsSunday,
        IsActive
    }

    private async Task<Result<bool>> UpdateNameAsync(
        string? name,
        Domain.Person.Person person,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<bool>.Success(false);
        var normalizedName = _service.NormalizeString(name);
        if (_service.HasOnlyLetters(normalizedName))
            return Result<bool>.Failure("El nombre del confirmando solo debe contener letras");
        var alreadyExists = await _person.AlreadyExistExceptIdAsync(normalizedName, person.ID, ct);
        if (alreadyExists)
            return Result<bool>.Failure("El confirmando ya existe");
        var changeResult = person.SetName(name, normalizedName);
        if (!changeResult.IsSuccess)
            return Result<bool>.Failure(changeResult.Error!);
        return Result<bool>.Success(true);
    }

    private Result<bool> UpdatePropertyString(
        string? value,
        Domain.Person.Person person,
        PropertyType type
    )
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<bool>.Success(false);
        if (type == PropertyType.Phone && !_service.HasOnlyNumbers(value))
            return Result<bool>.Failure("El número telefónico solo puede contener números");
        var result = type switch
        {
            PropertyType.Parish => person.SetParish(value),
            PropertyType.Phone => person.SetPhone(value),
            _ => person.SetAddress(value)
        };
        if (!result.IsSuccess)
            return Result<bool>.Failure(result.Error!);
        return Result<bool>.Success(true);
    }

    private static Result<bool> UpdatePropertyBool(
        bool? value,
        Domain.Person.Person person,
        PropertyType type
    )
    {
        if (value is null)
            return Result<bool>.Success(false);
        var result = type switch
        {
            PropertyType.IsActive => person.SetActive(value.Value),
            _ => person.SetDay(value.Value)
        };
        if (!result.IsSuccess)
            return Result<bool>.Failure(result.Error!);
        return Result<bool>.Success(true);
    }

    private static Result<bool> UpdateDOB(
        DateOnly? dob,
        Domain.Person.Person person
    )
    {
        if (dob is null)
            return Result<bool>.Success(false);
        var result = person.SetDOB(dob.Value);
        if (!result.IsSuccess)
            return Result<bool>.Failure(result.Error!);
        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> UpdateDegreeAsync(
        Guid? degreeId,
        Domain.Person.Person person,
        CancellationToken ct
    )
    {
        if (degreeId is null)
            return Result<bool>.Success(false);
        var exists = await _degree.GetByIdAsync(degreeId.Value, ct) is not null;
        if (!exists)
            return Result<bool>.Failure("El grado académico no existe");
        var result = person.SetDegree(degreeId.Value);
        if (!result.IsSuccess)
            return Result<bool>.Failure(result.Error!);
        return Result<bool>.Success(true);
    }

    public async Task<Result> HandleAsync(
        UpdatePersonCommand command,
        CancellationToken cancellationToken
    )
    {
        var person = await _person.GetByIdAsync(command.PersonId, cancellationToken);
        if (person is null)
            return Result.Failure("El confirmando no existe :0");
        if (!person.IsActive && command.IsActive != true)
            return Result.Failure("Debe activar el confirmando antes de actualizar sus datos");

        var changed = false;
        var results = new List<Result<bool>>(8)
        {
            UpdatePropertyBool(command.IsActive, person, PropertyType.IsActive),
            UpdatePropertyBool(command.IsSunday, person, PropertyType.IsSunday),
            await UpdateDegreeAsync(command.LastDegreeId, person, cancellationToken),
            await UpdateNameAsync(command.Name, person, cancellationToken),
            UpdateDOB(command.DOB, person),
            UpdatePropertyString(command.Phone, person, PropertyType.Phone),
            UpdatePropertyString(command.Address, person, PropertyType.Address),
            UpdatePropertyString(command.Parish, person, PropertyType.Parish)
        };
        foreach (var result in results)
        {
            if (!result.IsSuccess)
                return Result.Failure(result.Error!);
            if (result.Value) changed = true;
        }

        if (!changed)
            return Result.Failure("No se han encontrado datos para actualizar");
        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("Person {person} has been updated by user {user}", command.PersonId, command.UserId);
        return Result.Success();
    }
}