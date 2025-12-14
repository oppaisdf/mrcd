using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Role.AddRole;

public sealed record AddRoleCommand(
    Guid UserId,
    string RoleName
) : ICommand<Guid>;