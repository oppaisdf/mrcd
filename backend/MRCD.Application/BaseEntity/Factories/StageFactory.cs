using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;
using MRCD.Domain.Planner;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class StageFactory : IBaseEntityFactory<Stage>
{
    public Result<Stage> Create(
        string name
    ) => Stage.Create(name);
}