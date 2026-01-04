using MRCD.Domain.Common;

namespace MRCD.Domain.User;

public sealed class User
{
    private User() { }

    public Guid ID { get; private set; }
    public string Username { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public string Password { get; private set; } = default!;
    public DateOnly LastPasswordUpdate { get; private set; }

    public static Result<User> Create(
        string user,
        string pass
    )
    {
        if (string.IsNullOrWhiteSpace(user))
            return Result<User>.Failure("El usuario es requerido");
        if (user.Trim().Length > 10)
            return Result<User>.Failure("El usuario no puede superar los 10 caracteres");
        if (string.IsNullOrWhiteSpace(pass))
            return Result<User>.Failure("La contraseña del usuario es requerida");
        return Result<User>.Success(new()
        {
            ID = Guid.NewGuid(),
            Username = user.Trim(),
            IsActive = true,
            Password = pass,
            LastPasswordUpdate = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6))
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
        LastPasswordUpdate = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6));
        return Result.Success();
    }

    public Result SetActive(
        bool newActive
    )
    {
        if (newActive == IsActive)
            return Result.Failure("Se intenta cambiar al mismo estado");
        IsActive = newActive;
        return Result.Success();
    }

    public Result SetUsername(
        string userName
    )
    {
        if (string.IsNullOrWhiteSpace(userName))
            return Result.Failure("El nombre de usuario no puede ser nulo");
        if (userName.Trim().Length > 10)
            return Result.Failure("El usuario no puede superar los diez caracteres");
        Username = userName.Trim();
        return Result.Success();
    }
}