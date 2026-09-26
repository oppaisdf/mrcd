using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Add.Assign;

internal sealed class AssignPersonEntityHandler<TEntity>(
    IPersonRepository people,
    PersonEntityAssignments<TEntity> assignments,
    IPersistenceContext save
) : IBaseCommandHandler<AssignPersonEntityCommand, TEntity>
    where TEntity : Domain.Common.BaseEntity
{
    public async Task<Result> HandleAsync(
        AssignPersonEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var person = await people.GetByIdAsync(command.PersonId, cancellationToken);
        if (person is null || !person.IsActive)
            return Result.Failure("El confirmando no existe o se encuentra inactivo");
        var result = command.IsAssignation
            ? await assignments.AddAsync(command, person.Name, cancellationToken)
            : await assignments.DeleteAsync(command, person.Name, cancellationToken);
        if (result.IsSuccess) await save.SaveChangesAsync(cancellationToken);
        return result;
    }
}
