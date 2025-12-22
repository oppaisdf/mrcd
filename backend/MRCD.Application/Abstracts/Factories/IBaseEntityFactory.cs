using MRCD.Domain.Common;

namespace MRCD.Application.Abstracts.Factories;

public interface IBaseEntityFactory<TEntity>
{
    Result<TEntity> Create(string name);
}