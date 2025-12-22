using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;
using MRCD.Domain.Degree;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class DegreeFactory : IBaseEntityFactory<Degree>
{
    public Result<Degree> Create(
        string name
    ) => Degree.Create(name);
}