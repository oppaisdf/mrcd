namespace MRCD.Application.Abstracts.Security;

public sealed class EncryptionOptions
{
    /// <summary>
    /// Base64 de 32 bytes (AES-256).
    /// </summary>
    public required string KeyBase64 { get; init; }
    public string Version { get; init; } = "v1";
}