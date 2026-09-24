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
        var existsId = await entity.GetByIdAsync(command.EntityId, ct) is not null;
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
                if (await charge.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                charge.Add(new(command.PersonId, command.EntityId));
                break;
            case BaseEntityType.Document:
                if (await document.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                document.Add(new(command.PersonId, command.EntityId));
                break;
            case BaseEntityType.Sacrament:
                if (await sacrament.GetAsync(command.PersonId, command.EntityId, ct) is not null)
                    return Result.Failure(assignedError);
                sacrament.Add(new(command.PersonId, command.EntityId));
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
                var chg = await charge.GetAsync(command.PersonId, command.EntityId, ct);
                if (chg is null) return Result.Failure(existsError);
                charge.Remove(chg);
                break;
            case BaseEntityType.Document:
                var doc = await document.GetAsync(command.PersonId, command.EntityId, ct);
                if (doc is null) return Result.Failure(existsError);
                document.Remove(doc);
                break;
            case BaseEntityType.Sacrament:
                var sac = await sacrament.GetAsync(command.PersonId, command.EntityId, ct);
                if (sac is null) return Result.Failure(existsError);
                sacrament.Remove(sac);
                break;
            default:
                return Result.Failure("El tipo de entidad no es válido para esta asociación");
        }
        return Result.Success();
    }

}
