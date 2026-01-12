using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.GetGeneralList;

internal sealed class GetGeneralListHandler(
    IPersonRepository person,
    IParentRepository parent,
    ILogger<GetGeneralListHandler> logs
) : IQueryHandler<IEnumerable<GeneralListDTO>, GetGeneralListQuery>
{
    private readonly IPersonRepository _person = person;
    private readonly IParentRepository _parent = parent;
    private readonly ILogger<GetGeneralListHandler> _logs = logs;

    public async Task<Result<IEnumerable<GeneralListDTO>>> HandleAsync(
        GetGeneralListQuery query,
        CancellationToken cancellationToken
    )
    {
        var people = await _person.OnlyActiveToListAsync(cancellationToken);
        var parents = await _parent.FilteredByActivePersonToListAsync(cancellationToken);
        var parentsDir = parents
            .GroupBy(p => (p.PersonId, p.IsParent))
            .ToDictionary(
                g => g.Key,
                g => g.AsEnumerable()
            );
        var result = people
            .Select(p => new GeneralListDTO(
                p.Name,
                p.IsMasculine,
                p.IsSunday,
                p.DOB,
                p.RegistrationDate,
                p.Parish,
                p.Address,
                p.Phone,
                parentsDir.TryGetValue((p.ID, true), out var parents)
                    ? parents : [],
                parentsDir.TryGetValue((p.ID, false), out var godparents)
                    ? godparents : []
            ));
        _logs.LogInformation("User {user} has consulted the general list", query.UserId);
        return Result<IEnumerable<GeneralListDTO>>.Success(result);
    }
}