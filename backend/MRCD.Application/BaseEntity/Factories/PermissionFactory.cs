using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class PermissionFactory : IBaseEntityFactory<Domain.Role.Permission>
{
    Result<Domain.Role.Permission> IBaseEntityFactory<Domain.Role.Permission>.Create(
        string name
    ) => Domain.Role.Permission.Create(name);
}