using System.Text.RegularExpressions;
using api.Common;
using api.Context;
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

public partial class UserService(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    MerContext context,
    ICommonService common
) : IUserService
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly MerContext _context = context;
    private readonly ICommonService _common = common;

    #region "Private"
    private async Task AssignRolesAsync(
        IdentityUser user,
        List<string> roles
    )
    {
        var _roles = await _roleManager.Roles
            .AsNoTracking()
            .Where(r => r.Name != "sys")
            .Select(r => r.Name)
            .ToListAsync() ?? throw new DoesNotExistsException("Los roles no fueron encontrados");
        var cleanRoles = roles.Contains("adm") ? _roles! : roles.Where(_roles.Contains).ToList();
        if (cleanRoles == null || cleanRoles.Count == 0) throw new DoesNotExistsException("Los roles no fueron encontrados");

        var addRoleResult = await _userManager.AddToRolesAsync(user, cleanRoles);
        if (!addRoleResult.Succeeded)
            throw new Exception($"[+] Error al asignar roles al usuario {user.UserName}, verificar usuario y sus roles: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
    }

    [GeneratedRegex("^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{6,}$")]
    private partial Regex IsValidPasswordRegex();
    private bool IsValidPassword(string pass) => IsValidPasswordRegex().IsMatch(pass);
    private async Task AssignPassAsync(
        IdentityUser user,
        string pass
    )
    {
        if (!IsValidPassword(pass)) throw new BadRequestException("La contraseña debe tener, por lo menos, un número, una mayúscula y un caráacter especial");
        var result = await _userManager.AddPasswordAsync(user, pass);
        if (result.Succeeded) return;
        throw new Exception($"[+] Error al actualizar la contraseña del usuario {user.UserName}: {string.Join(",", result.Errors.Select(e => e.Description))}");
    }
    #endregion

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
            Email = "fake@fake.com",
            EmailConfirmed = true
        };

        var normalized = _common.GetNormalizedText(request.Username!);
        var alreadyExist = await _userManager.Users.AnyAsync(u => u.NormalizedUserName == normalized);
        if (alreadyExist) throw new AlreadyExistsException("El usuario ya existe");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            if (!IsValidPassword(request.Password!)) throw new BadRequestException("La contraseña debe tener, por lo menos, un número, una mayúscula y un caráacter especial");
            // Crea el Usuario
            var result = await _userManager.CreateAsync(user, request.Password!);
            if (!result.Succeeded) throw new Exception($"[+] Error al crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // Asigna los roles
            await AssignRolesAsync(user, request.Roles!);
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
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

        if (!user.EmailConfirmed && request.IsActive != true) throw new BadRequestException("Ese usuario está incativo, no se puede actualizar");

        if (request.Username != null && user.UserName != request.Username)
        {
            var normalized = _common.GetNormalizedText(request.Username);
            var alreadyExists = await _userManager.Users
                .AnyAsync(u => u.NormalizedUserName == normalized && u.Id != id);
            if (alreadyExists) throw new AlreadyExistsException("El usuario ya existe");
            user.UserName = request.Username;
        }

        if (request.IsActive != null && user.EmailConfirmed != request.IsActive) user.EmailConfirmed = request.IsActive!.Value;

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            if (request.Password != null) await AssignPassAsync(user, request.Password);
            if (request.Roles != null && request.Roles.Count != 0)
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                if (!removeRolesResult.Succeeded) throw new Exception($"[+] Error al remover roles de usuario {user.UserName}: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");
                await AssignRolesAsync(user, request.Roles);
            }

            // Actalización de los datos del usuario
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) throw new Exception($"[+] Error al actualizar usuario {user.UserName}: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}