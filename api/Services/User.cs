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
    Task DeleteAsync(string id);
    Task ActivateAsync(string id);
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
        var user = await _userManager.FindByNameAsync(request.Name) ?? throw new DoesNotExistsException("Invalid username or password");

        if (!user.EmailConfirmed) throw new DoesNotExistsException("Invalid username or password");
        var correctPassword = await _userManager.CheckPasswordAsync(user, request.Pass);
        if (!correctPassword) throw new DoesNotExistsException("Invalid username or password");

        var role = await _userManager.GetRolesAsync(user);
        await _signInManager.SignInAsync(user, false);
        return role;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
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
        if (userNameAlreadyExists != null) throw new AlreadyExistsException("Username already exists");

        // Crea el Usuario
        var result = await _userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded) throw new Exception($"[+] Error al crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        // Asigna los roles
        foreach (var role in request.Roles!)
        {
            if (role.Equals("sys")) throw new BadRequestException("This role cannot be assigned");

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist) throw new DoesNotExistsException("Role to be assigned does not exist");

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
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("Invalid User Id");
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
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("User Not Found");
        var sysUsers = await _userManager.GetUsersInRoleAsync("sys") ?? throw new Exception($"[+] Error al obtener los usuarios sys, actualizando el usuario {user.Id}");

        /*
        Se verifica que el usuario a modificar no sea un usuario sys
        Si el usuario es sys, se valida que el usuario que hace la petición también sea sys
        */
        if (sysUsers.Any(u => u.Id == id) && !sysUsers.Any(u => u.Id == userIdRequest))
            throw new BadRequestException("User cannot be changed!");

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email.Trim() != user.Email) user.Email = request.Email.Trim();
        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username.Trim() != user.UserName)
        {
            user.UserName = request.Username.Trim();
            var userNameAlreadyExists = await _userManager.FindByNameAsync(user.UserName);
            if (userNameAlreadyExists != null && userNameAlreadyExists.Id != user.Id)
                throw new AlreadyExistsException("Username already exists");
        }

        // Actalización de los datos del usuario
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) throw new Exception($"[+] Error al actualizar usuario {user.UserName}: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

        // Actualización de roles
        if (!sysUsers.Contains(user) && request.Roles!.Count > 0)
        {
            foreach (var role in request.Roles)
            {
                if (role.Equals("sys")) throw new BadRequestException("This role cannot be assigned");

                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (roleExists) continue;
                throw new DoesNotExistsException("Role Not Found");
            }

            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!removeRolesResult.Succeeded) throw new Exception($"[+] Error al remover roles de usuario {user.UserName}: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");

            var addRolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addRolesResult.Succeeded) throw new Exception($"[+] Error al asignar roles a usuario {request.Username}: {string.Join(", ", addRolesResult.Errors.Select(e => e.Description))}");
        }

        // Actualización de contraseña
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
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

    public async Task DeleteAsync(
        string id
    )
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("Invalid User Id");
        var roles = await _userManager.GetRolesAsync(user) ?? throw new Exception($"[+] Error al consultar roles de usuario en inactivación de usuario {id}");

        if (roles.Contains("sys")) throw new BadRequestException("This user cannot be inactivated");

        user.EmailConfirmed = false;
        var resultDelete = await _userManager.UpdateAsync(user);

        if (!resultDelete.Succeeded)
            throw new Exception($"[+] Error al inactivar al usuario {id}: {resultDelete.Errors.Select(e => e.Description)}");
    }

    public async Task ActivateAsync(
        string id
    )
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new DoesNotExistsException("Invalid User Id");

        user.EmailConfirmed = true;
        var resultDelete = await _userManager.UpdateAsync(user);

        if (!resultDelete.Succeeded)
            throw new Exception($"[+] Error al inactivar al usuario {id}: {resultDelete.Errors.Select(e => e.Description)}");
    }
}