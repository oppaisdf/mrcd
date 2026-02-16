using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Parent.GetparentById;

public sealed record GetParentByIdQuery(
    Guid ParentId,
    Guid UserId
) : IQuery<ParentDetailsDTO>;