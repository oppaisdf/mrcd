using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.Role.DTOs;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.GetUserById;

internal sealed class GetUserByIdHandler(
    IUserRepository user,
    IRoleRepository role,
    IUserRoleRepository userRole
) : IQueryHandler<UserDTO, GetUserByIdQuery>
{
    private readonly IUserRepository _user = user;
    private readonly IRoleRepository _role = role;
    private readonly IUserRoleRepository _userRole = userRole;

    public async Task<Result<UserDTO>> HandleAsync(
        GetUserByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var user = await _user.GetByIdAsync(query.Id, cancellationToken);
        var roles = await _role.ToListAsync(cancellationToken);
        var userRoles = await _userRole.RolesByUserIdToListAsync(query.Id, cancellationToken);

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