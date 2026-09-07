using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Permission.Add;

public sealed record AddPermissionCommand(
    string PermissionName
) : ICommand<Guid>;