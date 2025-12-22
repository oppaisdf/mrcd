using MRCD.Application.Abstracts.Handlers;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.DelBaseEntity;

public sealed record DelBaseEntityCommand(
    Guid UserId,
    Guid Id
) : ICommand<Result>;