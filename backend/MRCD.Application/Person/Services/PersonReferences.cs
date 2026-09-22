using MRCD.Application.BaseEntity.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;
using MRCD.Domain.Degree;

namespace MRCD.Application.Person.Services;

internal sealed class PersonReferences(
    IPersonRepository people,
    IBaseEntityRepository<Degree> degrees
)
{
    public async Task<Result> ValidateAsync(
        string? normalizedName,
        Guid? degreeId,
        Guid? exceptId,
        CancellationToken ct
    )
    {
        if (normalizedName is not null)
        {
            var exists = exceptId.HasValue
                ? await people.AlreadyExistExceptIdAsync(normalizedName, exceptId.Value, ct)
                : await people.AlreadyExistsNameAsync(normalizedName, ct);
            if (exists) return Result.Failure("El confirmando ya se ha registrado");
        }
        if (degreeId.HasValue && await degrees.GetByIdAsync(degreeId.Value, ct) is null)
            return Result.Failure("El grado académico no existe");
        return Result.Success();
    }
}
