using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Logs;
using MRCD.Application.Parent.Services;
using MRCD.Application.Person.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Add;

internal sealed class AddPersonHandler(
    PersonFactory factory,
    PersonReferences references,
    ParentResolver parents,
    PersonRegistration registration,
    IPersistenceContext save,
    AuditLog<AddPersonHandler> audit
) : ICommandHandler<AddPersonCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        AddPersonCommand command,
        CancellationToken cancellationToken
    )
    {
        var created = factory.Create(command);
        if (!created.IsSuccess) return Result<Guid>.Failure(created.Error!);
        var person = created.Value!;
        var valid = await references.ValidateAsync(
            person.NormalizedName,
            person.LastDegreeId,
            exceptId: null,
            cancellationToken
        );
        if (!valid.IsSuccess) return Result<Guid>.Failure(valid.Error!);
        var resolvedParents = await parents.ResolveManyAsync(command.Parents, cancellationToken);
        if (!resolvedParents.IsSuccess) return Result<Guid>.Failure(resolvedParents.Error!);
        var sacraments = await registration.ResolveSacramentsAsync(command.Sacraments, cancellationToken);
        if (!sacraments.IsSuccess) return Result<Guid>.Failure(sacraments.Error!);

        registration.Add(
            person,
            resolvedParents.Value!,
            sacraments.Value!
        );
        await save.SaveChangesAsync(cancellationToken);
        audit.Write(command.UserId, "Person {person} with ID {id} has been created.", person.Name, person.ID);
        return Result<Guid>.Success(person.ID);
    }
}
