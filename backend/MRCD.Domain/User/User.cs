using MRCD.Domain.Common;

namespace MRCD.Domain.User;

public sealed class User
{
    private User() { }

    public Guid Id { get; private set; }
    public string Username { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public string Password { get; private set; } = default!;

    public static Result<User> Create(
        string user,
        string pass
    )
    {
        if (string.IsNullOrWhiteSpace(user))
            return Result<User>.Failure("El usuario es requerido");
        if (string.IsNullOrWhiteSpace(pass))
            return Result<User>.Failure("La contraseña del usuario es requerida");
        return Result<User>.Success(new()
        {
            Id = Guid.NewGuid(),
            Username = user.Trim(),
            IsActive = true,
            Password = pass
        });
    }

    public Result SetPassword(
        string pass
    )
    {
        if (string.IsNullOrWhiteSpace(pass))
            return Result.Failure("La nueva contraseña es requerida");
        if (pass == Password)
            return Result.Failure("La contraseña debe ser diferente a la contraseña actual");
        Password = pass;
        return Result.Success();
    }
}