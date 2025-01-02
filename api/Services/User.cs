using api.Common;
using api.Models.Requests;
using api.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IUserService
{
    Task<IdentityUser> CreateAsync(UserRequest request);
    Task<ICollection<UserResponse>> GetAsync();
    Task<UserResponse> GetByIdAsync(string id);
    Task UpdateAsync(string id, UserRequest request, string userIdRequest);
    Task<ICollection<string>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}

public class UserService(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager
) : IUserService
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    #region "Login"
    public async Task<ICollection<string>> LoginAsync(
        LoginRequest request
    )
    {
        var user = await _userManager.FindByNameAsync(request.Name) ?? throw new DoesNotExistsException("Usuario o contraseña incorrecta");

        if (!user.EmailConfirmed) throw new DoesNotExistsException("Usuario o contraseña incorrecta");
        var correctPassword = await _userManager.CheckPasswordAsync(user, request.Pass);
        if (!correctPassword) throw new DoesNotExistsException("Usuario o contraseña incorrecta");

        var role = await _userManager.GetRolesAsync(user);
        await _signInManager.SignInAsync(user, false);
        return role;
    }

    public async Task LogoutAsync() => await _signInManager.SignOutAsync();
    #endregion

    public async Task<IdentityUser> CreateAsync(
            UserRequest request
    )
    {
        var user = new IdentityUser
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var userNameAlreadyExists = await _userManager.FindByNameAsync(user.UserName!);
        if (userNameAlreadyExists != null) throw new AlreadyExistsException("El usuario ya existe");

        // Crea el Usuario
        var result = await _userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded) throw new Exception($"[+] Error al crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        // Asigna los roles
        foreach (var role in request.Roles!)
        {
            if (role.Equals("sys")) throw new BadRequestException("Este rol sys no puede ser asignado");

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist) throw new DoesNotExistsException("El rol no existe para ser asignado");

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (addRoleResult.Succeeded) continue;
            throw new Exception($"[+] Error al asignar el rol {role} al usuario {user.UserName}, verificar usuario y sus roles: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
        }
        return user;
    }

    public async Task<ICollection<UserResponse>> GetAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersWithRole = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            usersWithRole.Add(new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsActive = user.EmailConfirmed,
                Roles = [.. roles]
            });
        }
        return usersWithRole;
    }

    public async Task<UserResponse> GetByIdAsync(
        string id
    )
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("El ID del usuario es inválido o no existe");
        var roles = await _userManager.GetRolesAsync(user) ?? throw new Exception($"[+] Error al buscar los roles del usuario {id}");

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            IsActive = user.EmailConfirmed,
            Roles = [.. roles]
        };
        return userResponse;
    }

    public async Task UpdateAsync(
        string id,
        UserRequest request,
        string userIdRequest
    )
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("Usuario no encontrado");
        var sysUsers = await _userManager.GetUsersInRoleAsync("sys") ?? throw new Exception($"[+] Error al obtener los usuarios sys, actualizando el usuario {user.Id}");
        if (!user.EmailConfirmed && request.IsActive != true) throw new BadRequestException("El usuario está inactivo, no se puede actualizar");

        /*
        Se verifica que el usuario a modificar no sea un usuario sys
        Si el usuario es sys, se valida que el usuario que hace la petición también sea sys
        */
        if (sysUsers.Any(u => u.Id == id) && !sysUsers.Any(u => u.Id == userIdRequest))
            throw new BadRequestException("El usuario sys no puede ser cambiado por otro usuario");

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email) user.Email = request.Email;
        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.UserName)
        {
            user.UserName = request.Username;
            var userNameAlreadyExists = await _userManager.FindByNameAsync(user.UserName);
            if (userNameAlreadyExists != null && userNameAlreadyExists.Id != user.Id)
                throw new AlreadyExistsException("El usuario ya existe");
        }

        if (request.IsActive != null && user.EmailConfirmed != request.IsActive)
        {
            var roles = await _userManager.GetRolesAsync(user) ?? throw new Exception($"[+] Error al consultar roles de usuario en inactivación de usuario {id}");
            if (roles.Contains("sys")) throw new BadRequestException("El usuario sys no puede ser inactivado");

            user.EmailConfirmed = request.IsActive!.Value;
        }

        // Actalización de los datos del usuario
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) throw new Exception($"[+] Error al actualizar usuario {user.UserName}: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

        // Actualización de roles
        if (sysUsers.Contains(user) || request.Roles!.Count == 0) goto UpdatePass;

        foreach (var role in request.Roles)
        {
            if (role.Equals("sys")) throw new BadRequestException("Este rol no puede ser asignado");

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (roleExists) continue;
            throw new DoesNotExistsException("El rol no existe");
        }

        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        if (!removeRolesResult.Succeeded) throw new Exception($"[+] Error al remover roles de usuario {user.UserName}: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");

        var addRolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
        if (!addRolesResult.Succeeded) throw new Exception($"[+] Error al asignar roles a usuario {request.Username}: {string.Join(", ", addRolesResult.Errors.Select(e => e.Description))}");

        UpdatePass:
        // Actualización de contraseña
        if (string.IsNullOrWhiteSpace(request.Password)) return;
        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
        if (!removePasswordResult.Succeeded)
            throw new Exception(
                $"[+] Error al actualizar la contraseña del usuario {request.Username}: {string.Join(", ", removePasswordResult.Errors.Select(e => e.Description))}"
            );

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.Password);
        if (!addPasswordResult.Succeeded) throw new Exception(
                $"[+] Error al actualizar la contraseña del usuario {request.Username}: {string.Join(",", addPasswordResult.Errors.Select(e => e.Description))}"
            );
    }
}