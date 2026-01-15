using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MRCD.API.Security;

internal sealed class PermissionPolicyProvider(
    IOptions<AuthorizationOptions> options
) : DefaultAuthorizationPolicyProvider(options)
{
    public const string Prefix = "perm:";

    public override Task<AuthorizationPolicy?> GetPolicyAsync(
        string policyName
    )
    {
        if (!policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            return base.GetPolicyAsync(policyName);

        var perm = policyName[Prefix.Length..];
        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(perm))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}