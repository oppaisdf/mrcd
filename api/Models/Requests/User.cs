using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests;

public class LoginRequest
{
    [Required]
    [MaxLength(15, ErrorMessage = "Nombre inválido")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(15, ErrorMessage = "Contraseña inválida")]
    public required string Pass { get; set; }
}

public class UserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public List<string>? Roles { get; set; }
    public string? Email { get; set; }
}