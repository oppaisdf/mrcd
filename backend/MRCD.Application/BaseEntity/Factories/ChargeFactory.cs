using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class ChargeFactory : IBaseEntityFactory<Domain.Charge.Charge>
{
    public Result<Domain.Charge.Charge> Create(
        string name
    ) => Domain.Charge.Charge.Create(name, 0);
}