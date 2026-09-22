using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Parent;

namespace MRCD.Application.Parent.Services;

internal sealed class ParentAssignments(
    IPersonRepository people,
    IParentPersonRepository links
)
{
    public async Task<Result> ValidateAsync(
        Guid personId,
        Guid parentId,
        bool isParent,
        CancellationToken ct
    )
    {
        if (!await people.ExistsActiveAsync(personId, ct))
            return Result.Failure("El confirmando/ahijado no existe o está inactivo");
        if (await links.GetAsync(personId, parentId, isParent, ct) is not null)
            return Result.Failure("El padre/padrino ya se ha asignado al confirmando");
        return ParentAssignmentRules.ValidateCount(await links.AssignedCountAsync(personId, isParent, ct) + 1);
    }

    public void Add(
        Guid personId,
        Guid parentId,
        bool isParent
    ) => links.Add(new(parentId, personId, isParent));

    public async Task<Result> RemoveAsync(
        Guid personId,
        Guid parentId,
        bool isParent,
        CancellationToken ct
    )
    {
        var link = await links.GetAsync(personId, parentId, isParent, ct);
        if (link is null) return Result.Failure("No se ha asignado el padre/padrino al confirmando");
        links.Del(link);
        return Result.Success();
    }
}
