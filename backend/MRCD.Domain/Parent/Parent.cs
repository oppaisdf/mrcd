using MRCD.Domain.Common;

namespace MRCD.Domain.Parent;

public sealed class Parent
{
    private Parent() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsMasculine { get; private set; }
    public string? Phone { get; private set; }

    public static Result<Parent> Create(
        string name,
        bool isMasculine,
        string? phone
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Parent>.Failure("El nombre del padre/padrino no puede estar vacío");
        if (string.IsNullOrWhiteSpace(phone)) phone = null;
        return Result<Parent>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim(),
            IsMasculine = isMasculine,
            Phone = phone?.Trim()
        });
    }
}