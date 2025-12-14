using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.DelPermission;

public sealed record DelPermissionCommand(
    Guid UserId,
    Guid PermissionId
) : ICommand<Result>;