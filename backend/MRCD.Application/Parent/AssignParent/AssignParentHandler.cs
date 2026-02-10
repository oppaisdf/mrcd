using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Parent;

namespace MRCD.Application.Parent.AssignParent;

internal sealed class AssignParentHandler(
    IPersonRepository person,
    IParentRepository parent,
    IParentPersonRepository repo,
    IPersistenceContext save
) : ICommandHandler<AssignParentCommand>
{
    private readonly IPersonRepository _person = person;
    private readonly IParentRepository _parent = parent;
    private readonly IParentPersonRepository _repo = repo;
    private readonly IPersistenceContext _save = save;

    private async Task<Result> AddAsync(
        AssignParentCommand command,
        CancellationToken ct
    )
    {
        var existsActive = await _person.ExistsActiveAsync(command.PersonId, ct);
        var existsParent = await _parent.ExistsAsync(command.ParentId, ct);
        if (!existsActive)
            return Result.Failure("El confirmando no existe o se encuentra inactivo");
        if (!existsParent)
            return Result.Failure("El padre/padrino no existe");
        var existing = await _repo.GetAsync(command.PersonId, command.ParentId, command.IsParent, ct);
        if (existing is not null)
            return Result.Failure("Ya se ha asignado el padre/padrino al confirmando");
        var parentPerson = new ParentPerson(
            command.ParentId,
            command.PersonId,
            command.IsParent
        );
        _repo.Add(parentPerson);
        return Result.Success();
    }

    private async Task<Result> DeleteAsync(
        AssignParentCommand command,
        CancellationToken ct
    )
    {
        var parentPerson = await _repo.GetAsync(command.PersonId, command.ParentId, command.IsParent, ct);
        if (parentPerson is null)
            return Result.Failure("No se ha asignado el padre/padrino al confirmando");
        _repo.Del(parentPerson);
        return Result.Success();
    }

    public async Task<Result> HandleAsync(
        AssignParentCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = command.IsAssignation
            ? await AddAsync(command, cancellationToken)
            : await DeleteAsync(command, cancellationToken);
        if (result.IsSuccess)
            await _save.SaveChangesAsync(cancellationToken);
        return result;
    }
}