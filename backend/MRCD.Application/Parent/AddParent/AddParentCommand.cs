using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Parent.AddParent;

public sealed record AddParentCommand(
    Guid UserId,
    string ParentName,
    bool IsMasculine,
    bool IsParent,
    string? Phone,
    Guid? PersonId
) : ICommand<Guid>;