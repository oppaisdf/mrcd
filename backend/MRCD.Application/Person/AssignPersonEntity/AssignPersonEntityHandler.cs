using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Common;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.AssignPersonEntity;

internal sealed class AssignPersonEntityHandler(
    IPersonRepository person,
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    IPersonSacramentRepository sacrament,
    IBaseEntityRepository<Domain.Document.Document> doc,
    IBaseEntityRepository<Domain.Sacrament.Sacrament> sac,
    IBaseEntityRepository<Domain.Charge.Charge> charg,
    IPersistenceContext save
) : ICommandHandler<AssignPersonEntityCommand>
{
    private readonly IPersonRepository _person = person;
    private readonly IPersonChargeRepository _charge = charge;
    private readonly IPersonDocumentRepository _document = document;
    private readonly IPersonSacramentRepository _sacrament = sacrament;
    private readonly IBaseEntityRepository<Domain.Document.Document> _doc = doc;
    private readonly IBaseEntityRepository<Domain.Sacrament.Sacrament> _sac = sac;
    private readonly IBaseEntityRepository<Domain.Charge.Charge> _charg = charg;
    private readonly IPersistenceContext _save = save;

    private async Task<Result> AddAsync(
        AssignPersonEntityCommand command,
        CancellationToken ct
    )
    {
        var existsId = command.Entity switch
        {
            BaseEntityType.Charge => await _charg.GetByIdAsync(command.EntityId, ct) is not null,
            BaseEntityType.Document => await _doc.GetByIdAsync(command.EntityId, ct) is not null,
            BaseEntityType.Sacrament => await _sac.GetByIdAsync(command.EntityId, ct) is not null,
            _ => true
        };
        var entityName = command.Entity switch
        {
            BaseEntityType.Charge => "cobro",
            BaseEntityType.Document => "documento",
            BaseEntityType.Sacrament => "sacramento",
            _ => ""
        };
        if (!existsId)
            return Result.Failure($"No existe el {entityName} para asociar al confirmando");
        var assignedError = $"El {entityName} ya ha sido asociado al confirmando";

        switch (command.Entity)
        {
            case BaseEntityType.Charge:
                if (await _charge.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _charge.Add(new(command.PersonId, command.EntityId));
                break;
            case BaseEntityType.Document:
                if (await _document.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _document.Add(new(command.PersonId, command.EntityId));
                break;
            case BaseEntityType.Sacrament:
                if (await _document.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _sacrament.Add(new(command.PersonId, command.EntityId));
                break;
        }
        return Result.Success();
    }

    private async Task<Result> DeleteAsync(
        AssignPersonEntityCommand command,
        CancellationToken ct
    )
    {
        var entityName = command.Entity switch
        {
            BaseEntityType.Charge => "cobro",
            BaseEntityType.Document => "documento",
            BaseEntityType.Sacrament => "sacramento",
            _ => ""
        };
        var existsError = $"No se ha registrado el {entityName} al confirmando";

        switch (command.Entity)
        {
            case BaseEntityType.Charge:
                var charge = await _charge.GetAsync(command.PersonId, command.EntityId, ct);
                if (charge is null) return Result.Failure(existsError);
                _charge.Remove(charge);
                break;
            case BaseEntityType.Document:
                var document = await _document.GetAsync(command.PersonId, command.EntityId, ct);
                if (document is null) return Result.Failure(existsError);
                _document.Remove(document);
                break;
            case BaseEntityType.Sacrament:
                var sacrament = await _sacrament.GetAsync(command.PersonId, command.EntityId, ct);
                if (sacrament is null) return Result.Failure(existsError);
                _sacrament.Remove(sacrament);
                break;
        }
        return Result.Success();
    }

    public async Task<Result> HandleAsync(
        AssignPersonEntityCommand command,
        CancellationToken cancellationToken
    )
    {
        var personActive = await _person.ExistsActiveAsync(command.PersonId, cancellationToken);
        if (!personActive)
            return Result.Failure("El confirmando no existe o se encuentra inactivo");

        var result = command.IsAssignation
            ? await AddAsync(command, cancellationToken)
            : await DeleteAsync(command, cancellationToken);

        if (result.IsSuccess)
            await _save.SaveChangesAsync(cancellationToken);
        return result;
    }
}