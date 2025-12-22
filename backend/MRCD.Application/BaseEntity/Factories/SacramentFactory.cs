using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;
using MRCD.Domain.Sacrament;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class SacramentFactory : IBaseEntityFactory<Sacrament>
{
    public Result<Sacrament> Create(
        string name
    ) => Sacrament.Create(name);
}