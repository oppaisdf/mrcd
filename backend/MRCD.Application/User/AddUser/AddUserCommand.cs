using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.User.AddUser;

public sealed record AddUserCommand(
    Guid UserId,
    string Username,
    string Password,
    IEnumerable<Guid> Roles
) : ICommand<Guid>;