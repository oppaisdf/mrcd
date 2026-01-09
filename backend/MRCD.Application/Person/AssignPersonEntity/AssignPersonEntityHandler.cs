using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.AssignPersonEntity;

internal sealed class AssignPersonEntityHandler<TEntity>(
    IPersonRepository person,
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    IPersonSacramentRepository sacrament,
    IBaseEntityRepository<TEntity> entity,
    IPersistenceContext save
) : ICommandHandler<AssignPersonEntityCommand>
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IPersonRepository _person = person;
    private readonly IPersonChargeRepository _charge = charge;
    private readonly IPersonDocumentRepository _document = document;
    private readonly IPersonSacramentRepository _sacrament = sacrament;
    private readonly IBaseEntityRepository<TEntity> _entity = entity;
    private readonly IPersistenceContext _save = save;

    private async Task<Result> AddAsync(
        AssignPersonEntityCommand command,
        CancellationToken ct
    )
    {
        var existsId = await _entity.ExistsIdAsync(command.EntityId, ct);
        var entityName = command.Entity switch
        {
            DTOs.PersonTypeEntity.Charge => "cobro",
            DTOs.PersonTypeEntity.Document => "documento",
            DTOs.PersonTypeEntity.Sacrament => "sacramento",
            _ => ""
        };
        if (!existsId)
            return Result.Failure($"No existe el {entityName} para asociar al confirmando");
        var assignedError = $"El {entityName} ya ha sido asociado al confirmando";

        switch (command.Entity)
        {
            case DTOs.PersonTypeEntity.Charge:
                if (await _charge.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _charge.Add(new(command.PersonId, command.EntityId));
                break;
            case DTOs.PersonTypeEntity.Document:
                if (await _document.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _document.Add(new(command.PersonId, command.EntityId));
                break;
            case DTOs.PersonTypeEntity.Sacrament:
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
            DTOs.PersonTypeEntity.Charge => "cobro",
            DTOs.PersonTypeEntity.Document => "documento",
            DTOs.PersonTypeEntity.Sacrament => "sacramento",
            _ => ""
        };
        var existsError = $"No se ha registrado el {entityName} al confirmando";

        switch (command.Entity)
        {
            case DTOs.PersonTypeEntity.Charge:
                var charge = await _charge.GetAsync(command.PersonId, command.EntityId, ct);
                if (charge is null) return Result.Failure(existsError);
                _charge.Remove(charge);
                break;
            case DTOs.PersonTypeEntity.Document:
                var document = await _document.GetAsync(command.PersonId, command.EntityId, ct);
                if (document is null) return Result.Failure(existsError);
                _document.Remove(document);
                break;
            case DTOs.PersonTypeEntity.Sacrament:
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