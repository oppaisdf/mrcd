using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.User.DTOs;

namespace MRCD.Application.User.GetUserById;

public sealed record GetUserByIdQuery(
    Guid Id
) : IQuery<UserDTO>;