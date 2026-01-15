using Microsoft.AspNetCore.Authorization;

namespace MRCD.API.Security;

internal sealed class PermissionRequirement(
    string permission
) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}