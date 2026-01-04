using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Common;
using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Parent.GetParent;

public sealed record GetParentQuery(
    int Page,
    int Size,
    string? ParentName
) : IQuery<Pagination<ParentDTO>>;