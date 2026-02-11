using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Services.CommonService;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.AddParent;

internal sealed class AddParentHandler(
    IParentRepository repo,
    ICommonService service,
    IPersonRepository person,
    IParentPersonRepository parentPerson,
    IPersistenceContext save,
    ILogger<AddParentHandler> logs
) : ICommandHandler<AddParentCommand, Guid>
{
    private readonly IParentRepository _repo = repo;
    private readonly ICommonService _service = service;
    private readonly IPersonRepository _person = person;
    private readonly IParentPersonRepository _parentPerson = parentPerson;
    private readonly IPersistenceContext _save = save;
    private readonly ILogger<AddParentHandler> _logs = logs;

    private async Task<Result> AssignAsync(
        Guid parentId,
        Guid? personId,
        bool isParent,
        CancellationToken ct
    )
    {
        if (personId is null) return Result.Success();
        var existsActive = await _person.ExistsActiveAsync(personId.Value, ct);
        if (!existsActive)
            return Result.Failure("El confirmando/ahijado no existe o está inactivo");
        var exists = await _parentPerson.GetAsync(personId.Value, parentId, isParent, ct);
        if (exists is not null)
            return Result.Failure("El padre/padrino ya se ha asignado al confirmando");
        _parentPerson.Add(new(
            parentId,
            personId.Value,
            isParent
        ));
        _logs.LogInformation("Parent {parent} has been assigned to person {person}", parentId, personId.Value);
        return Result.Success();
    }

    private async Task<Result<Guid>> CreateAsync(
        AddParentCommand command,
        string normalizedName,
        CancellationToken ct
    )
    {
        if (
            !string.IsNullOrWhiteSpace(command.Phone)
            && !_service.HasOnlyNumbers(command.Phone)
        ) return Result<Guid>.Failure("El número telefónico no es válido");

        var result = Domain.Parent.Parent.Create(
            command.ParentName,
            normalizedName,
            command.IsMasculine,
            command.Phone
        );
        if (!result.IsSuccess)
            return Result<Guid>.Failure(result.Error!);
        _repo.Add(result.Value!);

        var resultAssign = await AssignAsync(result.Value!.ID, command.PersonId, command.IsParent, ct);
        if (!resultAssign.IsSuccess)
            return Result<Guid>.Failure(resultAssign.Error!);

        _logs.LogInformation("Parent {parent} has been added by user {user}", result.Value!.ID, command.UserId);
        return Result<Guid>.Success(result.Value!.ID);
    }

    public async Task<Result<Guid>> HandleAsync(
        AddParentCommand command,
        CancellationToken cancellationToken
    )
    {
        var normalizedName = _service.NormalizeString(command.ParentName);
        if (_service.HasOnlyLetters(normalizedName))
            return Result<Guid>.Failure("El nombre del padre/padrino solo puede contener letras");
        var currentParent = await _repo.GetByNameAsync(normalizedName, cancellationToken);

        if (currentParent is not null && command.PersonId is null)
            return Result<Guid>.Failure("El nombre del padre/padrino ya se ha registrado");

        Guid id;
        if (currentParent is null)
        {
            var resultCreate = await CreateAsync(command, normalizedName, cancellationToken);
            if (!resultCreate.IsSuccess)
                return Result<Guid>.Failure(resultCreate.Error!);
            id = resultCreate.Value;
        }
        else
        {
            var resultAssign = await AssignAsync(currentParent.ID, command.PersonId, command.IsParent, cancellationToken);
            if (!resultAssign.IsSuccess)
                return Result<Guid>.Failure(resultAssign.Error!);
            id = currentParent.ID;
        }
        await _save.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(id);
    }
}