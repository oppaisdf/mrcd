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

    public async Task<Result<Guid>> HandleAsync(
        AddParentCommand command,
        CancellationToken cancellationToken
    )
    {
        if (
            !string.IsNullOrWhiteSpace(command.Phone)
            && !_service.HasOnlyNumbers(command.Phone.Trim())
        )
            return Result<Guid>.Failure("El número telefónico no es válido");
        var parentResult = Domain.Parent.Parent.Create(
            command.ParentName,
            _service.NormalizeString(command.ParentName),
            command.IsMasculine,
            command.Phone
        );
        if (!parentResult.IsSuccess)
            return Result<Guid>.Failure(parentResult.Error!);
        var parent = parentResult.Value!;
        _repo.Add(parent);

        var alreadyExists = await _repo.AlreadyExists(parent.NormalizedName, cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Failure("El padre/padrino ya se ha registrado");

        if (command.PersonId is not null)
        {
            var personIsActive = await _person.ExistsActiveAsync(command.PersonId.Value, cancellationToken);
            if (!personIsActive)
                return Result<Guid>.Failure("El confirmando/ahijado no existe o está inactivo");
            _parentPerson.Add(new Domain.Parent.ParentPerson(
                parent.ID,
                command.PersonId.Value,
                command.IsParent
            ));
        }

        await _save.SaveChangesAsync(cancellationToken);
        _logs.LogInformation("Parent {parent} has been added by user {user}", parent.ID, command.UserId);
        return Result<Guid>.Success(parent.ID);
    }
}