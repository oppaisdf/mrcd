using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Logs;
using MRCD.Application.Parent.Services;
using MRCD.Application.Parent.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.Add;

internal sealed class AddParentHandler(
    ParentResolver parents,
    IParentRepository repository,
    ParentAssignments assignments,
    IPersistenceContext save,
    AuditLog<AddParentHandler> audit
) : ICommandHandler<AddParentCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddParentCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await parents.ResolveAsync(
            command.ParentName,
            command.IsMasculine,
            command.Phone,
            cancellationToken
        );
        if (!result.IsSuccess) return Result<Guid>.Failure(result.Error!);

        var parent = result.Value!;
        if (!parent.IsNew && command.PersonId is null)
            return Result<Guid>.Failure("El nombre del padre/padrino ya se ha registrado");

        if (command.PersonId is Guid personId)
        {
            var valid = await assignments.ValidateAsync(
                personId,
                parent.Entity.ID,
                command.IsParent,
                cancellationToken
            );
            if (!valid.IsSuccess) return Result<Guid>.Failure(valid.Error!);
        }

        if (parent.IsNew) repository.Add(parent.Entity);
        if (command.PersonId is Guid targetId)
            assignments.Add(
                targetId,
                parent.Entity.ID,
                command.IsParent
            );
        await save.SaveChangesAsync(cancellationToken);
        if (parent.IsNew)
            audit.Write(command.UserId, "Parent {parent} with ID {id} has been created.", parent.Entity.Name, parent.Entity.ID);
        if (command.PersonId is Guid assignedId)
            audit.Write(command.UserId, "Parent {parent} has been assigned to person {person}", parent.Entity.ID, assignedId);
        return Result<Guid>.Success(parent.Entity.ID);
    }
}
