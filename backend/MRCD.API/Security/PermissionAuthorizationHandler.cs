using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MRCD.Application.Security;

namespace MRCD.API.Security;

internal sealed class PermissionAuthorizationHandler(
    PermissionService service
) : AuthorizationHandler<PermissionRequirement>
{
    private readonly PermissionService _service = service;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        var sub = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? context.User.FindFirstValue("sub");

        if (!Guid.TryParse(sub, out var userId))
            return;

        if (await _service.HasPermissionAsync(userId, requirement.Permission, CancellationToken.None))
            context.Succeed(requirement);
    }
}