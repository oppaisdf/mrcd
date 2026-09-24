using MRCD.Application.Abstracts.Factories;
using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.BaseEntity.Services;

internal sealed class NamedEntityPreparation<TEntity>(
    IBaseEntityRepository<TEntity> repository,
    IBaseEntityFactory<TEntity> factory
)
    where TEntity : Domain.Common.BaseEntity
{
    public async Task<Result<TEntity>> PrepareAsync(
        string name,
        CancellationToken ct
    )
    {
        var normalized = StringNormalizer.NormalizeString(name);
        if (!StringValidator.HasOnlyLetters(normalized))
            return Result<TEntity>.Failure("El nombre solo debe contener letras");
        var created = factory.Create(name.Trim());
        if (!created.IsSuccess) return created;
        if ((await repository.ToListAsync(ct))
            .Any(r => StringNormalizer.NormalizeString(r.Name) == normalized)
        ) return Result<TEntity>.Failure("El nombre ya se encuentra en uso");
        return created;
    }
}
