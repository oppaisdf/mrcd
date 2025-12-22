using MRCD.Domain.Common;

namespace MRCD.Domain.Parent;

public sealed class Parent
{
    private Parent() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;
    public string NormalizedName { get; private set; } = default!;
    public bool IsMasculine { get; private set; }
    public string? Phone { get; private set; }

    public static Result<Parent> Create(
        string name,
        string normalizedName,
        bool isMasculine,
        string? phone
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Parent>.Failure("El nombre del padre/padrino no puede estar vacío");
        if (string.IsNullOrWhiteSpace(normalizedName))
            return Result<Parent>.Failure("El nombre normalizado del padre/padrino no puede estar vacío");
        if (name.Trim().Length > 80)
            return Result<Parent>.Failure("El nombre del padre/padrino no puede exceder los 80 caracteres");
        if (normalizedName.Trim().Length > 80 || normalizedName.Trim().Length > name.Trim().Length)
            return Result<Parent>.Failure("El nombre normalizado del padre/padrino no puede exceder la longitud del nombre original ni puede ser mayor a los 80 caracteres");
        if (string.IsNullOrWhiteSpace(phone)) phone = null;
        return Result<Parent>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim(),
            NormalizedName = normalizedName,
            IsMasculine = isMasculine,
            Phone = phone?.Trim()
        });
    }
}