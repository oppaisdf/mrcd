using api.Common;
using api.Context;
using api.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IRoleService
{
    Task CreateAsync(string name);
    Task<ICollection<RoleResponse>> GetAsync();
}

public class RoleService(
    RoleManager<IdentityRole> roleManager,
    UserManager<IdentityUser> userManager,
    MerContext context
) : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly MerContext _context = context;

    public async Task CreateAsync(
        string name
    )
    {
        var rolExist = await _roleManager.RoleExistsAsync(name);
        if (rolExist) throw new AlreadyExistsException("Role already exists! :0");

        using var tran = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded) throw new Exception($"[+] Error al crear el rol {name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var users = await _userManager.GetUsersInRoleAsync("adm") ?? throw new Exception($"[+] Error al asignar el rol {name} a los usuarios adm");
            foreach (var user in users)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, name);
                if (addRoleResult.Succeeded) continue;
                throw new Exception($"[+] Error al asignar el rol {name} al usuairo {user.UserName}");
            }

            await tran.CommitAsync();
        }
        catch (Exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<RoleResponse>> GetAsync()
    {
        var roles = await _roleManager.Roles
            .AsNoTracking()
            .Select(r => new RoleResponse
            {
                Id = r.Id,
                Name = r.Name!
            }).ToListAsync() ?? throw new DoesNotExistsException("Los roles no fueron encontrados");
        return roles;
    }
}