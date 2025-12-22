using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class RoleFactory : IBaseEntityFactory<Domain.Role.Role>
{
    public Result<Domain.Role.Role> Create(
        string name
    ) => Domain.Role.Role.Create(name);
}