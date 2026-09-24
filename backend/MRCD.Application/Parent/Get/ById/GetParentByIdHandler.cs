using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.Contracts;
using MRCD.Application.Parent.DTOs;
using MRCD.Application.Person.Contracts;
using MRCD.Application.Person.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.Get.ById;

internal sealed class GetParentByIdHandler(
    IParentRepository repo,
    IPersonRepository person,
    AuditLog<GetParentByIdHandler> logs
) : IQueryHandler<ParentDetailsDTO, GetParentByIdQuery>
{
    public async Task<Result<ParentDetailsDTO>> HandleAsync(
        GetParentByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var parent = await repo.GetByIdAsync(query.ParentId, cancellationToken);
        if (parent is null)
            return Result<ParentDetailsDTO>.Failure("El padre/padrino no existe");
        var people = await person.ByPaerentToListAsync(query.ParentId, cancellationToken);
        logs.Write(query.UserId, "Parent {parent} with ID {id} has been consulted.", parent.Name, query.ParentId);
        return Result<ParentDetailsDTO>.Success(new(
            parent.Name,
            parent.IsMasculine,
            parent.Phone,
            people.Where(p => p.IsChild).Select(p => new SimplePersonDTO(p.ID, p.Name)),
            people.Where(p => !p.IsChild).Select(p => new SimplePersonDTO(p.ID, p.Name))
        ));
    }
}