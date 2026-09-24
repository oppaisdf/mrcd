using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Role.Contracts;
using MRCD.Application.User.Contracts;
using MRCD.Application.User.DTOs;
using MRCD.Domain.Common;

namespace MRCD.Application.User.Login;

internal sealed class UserLoginHandler(
    IUserRepository userRepo,
    IRoleRepository role
) : ICommandHandler<UserLoginCommand, LoginDTO>
{
    public async Task<Result<LoginDTO>> HandleAsync(
        UserLoginCommand command,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return Result<LoginDTO>.Failure("El usuario es requerido");
        var user = await userRepo.GetByUsernameAsync(command.Username.Trim(), cancellationToken);
        if (user is null || !user.IsActive || !user.Password.Equals(command.Password))
            return Result<LoginDTO>.Failure("Credenciales inválidas");

        var roles = await role.ByUserIdToListAsync(user.ID, cancellationToken);
        return Result<LoginDTO>.Success(new(
            user.ID,
            roles.Select(r => r.Name)
        ));
    }
}