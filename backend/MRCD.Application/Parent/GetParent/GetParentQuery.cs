using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Common;
using MRCD.Application.Parent.DTOs;

namespace MRCD.Application.Parent.GetParent;

public sealed record GetParentQuery(
    int Page,
    int Size,
    string? ParentName,
    AlertType? Alert
) : IQuery<Pagination<ParentDTO>>;