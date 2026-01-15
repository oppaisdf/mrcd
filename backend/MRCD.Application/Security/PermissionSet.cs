namespace MRCD.Application.Security;

public sealed record PermissionSet(
    IReadOnlySet<string> Values
)
{
    public bool Contains(
        string permission
    ) => Values.Contains(permission);
};