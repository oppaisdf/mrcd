using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
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
                p.ID,
                p.Name,
                p.IsMasculine,
                p.IsSunday,
                p.DOB,
                p.RegistrationDate,
                p.Parish,
                p.Phone,
                parentsDir.TryGetValue((p.ID, true), out var parents)
                    ? parents.Select(p => new SimpleParentDTO(p.ParentName, p.Phone)) : [],
                parentsDir.TryGetValue((p.ID, false), out var godparents)
                    ? godparents.Select(p => new SimpleParentDTO(p.ParentName, p.Phone)) : []
            ));
        using (_logs.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = query.UserId
        }))
        {
            _logs.LogInformation("General list has been consulted");
        }
        return Result<IEnumerable<GeneralListDTO>>.Success(result);
    }
}