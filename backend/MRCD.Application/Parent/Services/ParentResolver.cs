using MRCD.Application.Parent.Contracts;
using MRCD.Application.Person.Add;
using MRCD.Application.Services;
using MRCD.Application.Services.Common;
using MRCD.Domain.Common;
using MRCD.Domain.Parent;

namespace MRCD.Application.Parent.Services;

internal sealed record ResolvedParent(
    Domain.Parent.Parent Entity,
    bool IsNew
);

// Resolves and creates detached entities. No repository writes or commits occur here.
internal sealed class ParentResolver(
    IParentRepository parents,
    ICommonService normalizer
)
{
    public async Task<Result<ResolvedParent>> ResolveAsync(
        string name,
        bool isMasculine,
        string? phone,
        CancellationToken ct
    )
    {
        var normalized = normalizer.NormalizeString(name);
        if (!StringValidator.HasOnlyLetters(normalized))
            return Result<ResolvedParent>.Failure("El nombre del padre/padrino solo puede contener letras");
        var existing = await parents.GetByNameAsync(normalized, ct);
        if (existing is not null)
            return Result<ResolvedParent>.Success(new(existing, false));
        var result = Domain.Parent.Parent.Create(
            name,
            normalized,
            isMasculine,
            string.IsNullOrWhiteSpace(phone) ? null : phone.Trim()
        );
        return result.IsSuccess
            ? Result<ResolvedParent>.Success(new(result.Value!, true))
            : Result<ResolvedParent>.Failure(result.Error!);
    }

    public async Task<Result<IReadOnlyList<ResolvedParent>>> ResolveManyAsync(
        IEnumerable<AddSimpleParentCommand>? input,
        CancellationToken ct
    )
    {
        if (input is null)
            return Result<IReadOnlyList<ResolvedParent>>.Failure("La lista de padres es requerida");
        var items = input.ToList();
        if (items.Any(p => p is null))
            return Result<IReadOnlyList<ResolvedParent>>.Failure("Los datos de los padres son requeridos");

        var distinct = items
            .DistinctBy(p => normalizer.NormalizeString(p.Name))
            .ToList();
        var count = ParentAssignmentRules.ValidateCount(distinct.Count);
        if (!count.IsSuccess)
            return Result<IReadOnlyList<ResolvedParent>>.Failure(count.Error!);
        var result = new List<ResolvedParent>();

        foreach (var item in distinct)
        {
            var resolved = await ResolveAsync(
                item.Name,
                item.IsMasculine,
                item.Phone,
                ct
            );
            if (!resolved.IsSuccess)
                return Result<IReadOnlyList<ResolvedParent>>.Failure(resolved.Error!);
            result.Add(resolved.Value!);
        }
        return Result<IReadOnlyList<ResolvedParent>>.Success(result);
    }

}
