using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Update;

public sealed record UpdateUserCommand(
    Guid UserId,
    Guid Id,
    string? Username = null,
    string? Password = null,
    bool? IsActive = null
) : ICommand<Result>;