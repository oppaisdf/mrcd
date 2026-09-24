using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Get.ById;

internal sealed class GetUserByIdHandler(
    IUserRepository userRepo,
    IRoleRepository role,
    IUserRoleRepository userRole
) : IQueryHandler<UserDTO, GetUserByIdQuery>
{
    public async Task<Result<UserDTO>> HandleAsync(
        GetUserByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepo.GetByIdAsync(query.Id, cancellationToken);
        var roles = await role.ToListAsync(cancellationToken);
        var userRoles = await userRole.RolesByUserIdToListAsync(query.Id, cancellationToken);

        var assigned = userRoles
            .Select(ur => ur.RoleID)
            .ToHashSet();

        if (user is null)
            return Result<UserDTO>.Failure("El usuario no existe");
        return Result<UserDTO>.Success(new UserDTO(
            user.ID,
            user.Username,
            user.IsActive,
            roles.Select(r => new UsingRoleDTO(
                r.ID,
                r.Name,
                assigned.Contains(r.ID)
            ))
        ));
    }
}