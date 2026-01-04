using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Parent.DelParent;

public sealed record DelParentCommand(
    Guid UserId,
    Guid ParentId
) : ICommand<Result>;