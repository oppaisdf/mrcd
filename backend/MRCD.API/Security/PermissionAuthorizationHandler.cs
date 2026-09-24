using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MRCD.Application.Security;

namespace MRCD.API.Security;

internal sealed class PermissionAuthorizationHandler(
    PermissionService service,
    IHttpContextAccessor http
) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        var sub = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? context.User.FindFirstValue("sub");

        if (!Guid.TryParse(sub, out var userId)) return;
        var ct = http.HttpContext?.RequestAborted ?? CancellationToken.None;
        if (await service.HasPermissionAsync(userId, requirement.Permission, ct))
            context.Succeed(requirement);
    }
}