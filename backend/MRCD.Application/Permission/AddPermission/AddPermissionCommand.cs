using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Permission.AddPermission;

public sealed record AddPermissionCommand(
    string PermissionName
) : ICommand<Guid>;