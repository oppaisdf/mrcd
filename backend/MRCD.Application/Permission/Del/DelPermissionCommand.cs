using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.Permission.Del;

public sealed record DelPermissionCommand(
    Guid UserId,
    Guid PermissionId
) : ICommand<Result>;