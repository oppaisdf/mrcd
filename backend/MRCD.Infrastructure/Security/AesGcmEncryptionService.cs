using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using MRCD.Application.Abstracts.Security;

namespace MRCD.Infrastructure.Security;

internal sealed class AesGcmEnryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly string _version;

    public AesGcmEnryptionService(
        IOptions<EncryptionOptions> opt
    )
    {
        var options = opt.Value;
        _key = Convert.FromBase64String(options.KeyBase64);
        if (_key.Length != 32) throw new InvalidOperationException($"EncryptionOptions.KeyBase64 debe decodificar a 32 bytes (AES-256). Actual: {_key.Length}");
        _version = options.Version;
    }

    public string? Decrypt(
        string? payload
    )
    {
        if (payload is null) return null;
        var parts = payload.Split(':', 2);
        if (parts.Length != 2)
            throw new CryptographicException("Payload inválido (no contiene versión).");
        var packed = Convert.FromBase64String(parts[1]);
        if (packed.Length < 12 + 16)
            throw new CryptographicException("Payload inválido (demasiado corto).");

        var nonce = packed.AsSpan(0, 12).ToArray();
        var tag = packed.AsSpan(12, 16).ToArray();
        var ciphertext = packed.AsSpan(28).ToArray();
        var plaintext = new byte[ciphertext.Length];

        using var aes = new AesGcm(_key, 16);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        return Encoding.UTF8.GetString(plaintext);
    }

    public string? Encrypt(
        string? plaintext
    )
    {
        if (plaintext is null) return null;
        var nonce = RandomNumberGenerator.GetBytes(12);
        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(_key, 16);
        aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

        // Payload: v1:<base64(nonce|tag|ciphertext)>
        var packed = new byte[nonce.Length + tag.Length + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, packed, 0, nonce.Length);
        Buffer.BlockCopy(tag, 0, packed, nonce.Length, tag.Length);
        Buffer.BlockCopy(ciphertext, 0, packed, nonce.Length + tag.Length, ciphertext.Length);
        return $"{_version}:{Convert.ToBase64String(packed)}";
    }
}