using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Role.Add;

public sealed record AddRoleCommand(
    Guid UserId,
    string RoleName
) : ICommand<Guid>;