using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.User.DTOs;

namespace MRCD.Application.User.Get.ById;

public sealed record GetUserByIdQuery(
    Guid Id
) : IQuery<UserDTO>;