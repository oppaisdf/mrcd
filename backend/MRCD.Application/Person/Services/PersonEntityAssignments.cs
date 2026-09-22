using MRCD.Application.Person.Add.Assign;
using MRCD.Application.BaseEntity.Common;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Services;

internal sealed class PersonEntityAssignments<TEntity>(
    IPersonChargeRepository charge,
    IPersonDocumentRepository document,
    IPersonSacramentRepository sacrament,
    IBaseEntityRepository<TEntity> entity
)
    where TEntity : Domain.Common.BaseEntity
{
    private readonly IPersonChargeRepository _charge = charge;
    private readonly IPersonDocumentRepository _document = document;
    private readonly IPersonSacramentRepository _sacrament = sacrament;
    private readonly IBaseEntityRepository<TEntity> _entity = entity;

    private static bool Matches(BaseEntityType entity) => entity switch
    {
        BaseEntityType.Charge => typeof(TEntity) == typeof(Domain.Charge.Charge),
        BaseEntityType.Document => typeof(TEntity) == typeof(Domain.Document.Document),
        BaseEntityType.Sacrament => typeof(TEntity) == typeof(Domain.Sacrament.Sacrament),
        _ => false
    };

    public async Task<Result> AddAsync(
        AssignPersonEntityCommand command,
        CancellationToken ct
    )
    {
        if (!Matches(command.Entity)) return Result.Failure("El tipo de entidad no es válido para esta asociación");
        var existsId = await _entity.GetByIdAsync(command.EntityId, ct) is not null;
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
                if (await _sacrament.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                _sacrament.Add(new(command.PersonId, command.EntityId));
                break;
            default:
                return Result.Failure("El tipo de entidad no es válido para esta asociación");
        }
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        AssignPersonEntityCommand command,
        CancellationToken ct
    )
    {
        if (!Matches(command.Entity)) return Result.Failure("El tipo de entidad no es válido para esta asociación");
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
            default:
                return Result.Failure("El tipo de entidad no es válido para esta asociación");
        }
        return Result.Success();
    }

}
