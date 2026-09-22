using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Logs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.Services;
using MRCD.Application.Services.Common;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Update;

internal sealed class UpdatePersonHandler(
    IPersonRepository people,
    ICommonService normalizer,
    PersonReferences references,
    IPersistenceContext save,
    AuditLog<UpdatePersonHandler> audit
) : ICommandHandler<UpdatePersonCommand>
{
    public async Task<Result> HandleAsync(
        UpdatePersonCommand command,
        CancellationToken cancellationToken
    )
    {
        var person = await people.GetByIdAsync(command.PersonId, cancellationToken);
        if (person is null) return Result.Failure("El confirmando no existe :0");
        var normalizedName = string.IsNullOrWhiteSpace(command.Name)
            ? null
            : normalizer.NormalizeString(command.Name);
        var valid = await references.ValidateAsync(
            normalizedName,
            command.LastDegreeId,
            person.ID,
            cancellationToken
        );
        if (!valid.IsSuccess) return valid;

        var updated = person.Update(
            command.Name,
            normalizedName,
            command.DOB,
            command.IsActive,
            command.IsSunday,
            command.Parish,
            command.Address,
            command.Phone,
            command.LastDegreeId
        );
        if (!updated.IsSuccess) return updated;
        await save.SaveChangesAsync(cancellationToken);
        audit.Write(command.UserId, "Person {person} with ID {id} has been updated.", person.Name, person.ID);
        return Result.Success();
    }
}
