using MRCD.Domain.Common;

namespace MRCD.Domain.User;

public sealed record Role
{
    private Role() { }
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;

    public static Result<Role> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Role>.Failure("El nombre del rol no puede venir vacío");
        return Result<Role>.Success(new()
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        });
    }
}