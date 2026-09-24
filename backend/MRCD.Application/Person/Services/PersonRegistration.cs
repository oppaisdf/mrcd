using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Parent.Services;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Sacrament;

namespace MRCD.Application.Person.Services;

internal sealed class PersonRegistration(
    IPersonRepository people,
    IParentRepository parents,
    ParentAssignments assignments,
    IBaseEntityRepository<Sacrament> sacraments,
    IPersonSacramentRepository links)
{
    public async Task<Result<IReadOnlyList<Guid>>> ResolveSacramentsAsync(
        IEnumerable<Guid>? ids,
        CancellationToken ct
    )
    {
        if (ids is null) return Result<IReadOnlyList<Guid>>.Success([]);
        var available = await sacraments.ToListAsync(ct);
        // Preserve the existing API behavior: unknown IDs are ignored.
        return Result<IReadOnlyList<Guid>>.Success([.. ids.Intersect(available.Select(s => s.ID))]);
    }

    public void Add(
        Domain.Person.Person person,
        IReadOnlyList<ResolvedParent> resolvedParents,
        IReadOnlyList<Guid> sacramentIds
    )
    {
        people.Add(person);
        foreach (var parent in resolvedParents)
        {
            if (parent.IsNew) parents.Add(parent.Entity);
            assignments.Add(person.ID, parent.Entity.ID, true);
        }

        if (sacramentIds.Count == 0) return;
        links.AddRange(
            sacramentIds.Select(id => new Domain.Person.PersonSacrament(person.ID, id))
        );
    }
}
