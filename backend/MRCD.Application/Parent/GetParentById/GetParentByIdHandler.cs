using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.GetparentById;

internal sealed class GetParentByIdHandler(
    IParentRepository repo,
    IPersonRepository person,
    ILogger<GetParentByIdHandler> logs
) : IQueryHandler<ParentDetailsDTO, GetParentByIdQuery>
{
    private readonly IParentRepository _repo = repo;
    private readonly IPersonRepository _person = person;
    private readonly ILogger<GetParentByIdHandler> _logs = logs;

    public async Task<Result<ParentDetailsDTO>> HandleAsync(
        GetParentByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var parent = await _repo.GetByIdAsync(query.ParentId, cancellationToken);
        if (parent is null)
            return Result<ParentDetailsDTO>.Failure("El padre/padrino no existe");
        var people = await _person.ByPaerentToListAsync(query.ParentId, cancellationToken);
        _logs.LogInformation("Parent {parent} has been consulted by user {user}", query.ParentId, query.UserId);
        return Result<ParentDetailsDTO>.Success(new(
            parent.Name,
            parent.IsMasculine,
            parent.Phone,
            people.Where(p => p.IsChild).Select(p => new SimplePersonDTO(p.ID, p.Name)),
            people.Where(p => !p.IsChild).Select(p => new SimplePersonDTO(p.ID, p.Name))
        ));
    }
}