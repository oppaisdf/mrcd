using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.Add.Assign;

internal sealed class AssignParentHandler(
    IParentRepository parents,
    ParentAssignments assignments,
    IPersistenceContext save
) : ICommandHandler<AssignParentCommand>
{
    public async Task<Result> HandleAsync(
        AssignParentCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.IsAssignation)
        {
            if (!await parents.ExistsAsync(command.ParentId, cancellationToken))
                return Result.Failure("El padre/padrino no existe");
            var valid = await assignments.ValidateAsync(
                command.PersonId,
                command.ParentId,
                command.IsParent,
                cancellationToken
            );
            if (!valid.IsSuccess) return valid;
            assignments.Add(
                command.PersonId,
                command.ParentId,
                command.IsParent
            );
        }
        else
        {
            var removed = await assignments.RemoveAsync(
                command.PersonId,
                command.ParentId,
                command.IsParent,
                cancellationToken
            );
            if (!removed.IsSuccess) return removed;
        }
        await save.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
