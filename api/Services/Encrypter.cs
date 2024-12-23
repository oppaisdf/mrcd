using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.Services;

public class EncryptionConverter : ValueConverter<string?, byte[]?>
{
    private static byte[]? Key;

    public static void InitializeKey(string key)
    {
        if (string.IsNullOrEmpty(key) || key.Length != 32)
            throw new ArgumentException("La clave debe tener 32 caracteres para AES-256.");

        Key = Encoding.UTF8.GetBytes(key);
    }

    public EncryptionConverter()
        : base(
            plainText => Encrypt(plainText),
            cipherBytes => Decrypt(cipherBytes))
    {
        if (Key == null)
            throw new InvalidOperationException("Encryption key not initialized.");
    }

    private static byte[]? Encrypt(string? plainText)
    {
        if (plainText == null)
            return null;

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key!;
        aesAlg.GenerateIV();
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        using MemoryStream msEncrypt = new();
        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

        using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }

        return msEncrypt.ToArray();
    }

    private static string? Decrypt(byte[]? cipherBytes)
    {
        if (cipherBytes == null)
            return null;

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key!;

        byte[] iv = new byte[16];
        Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
        aesAlg.IV = iv;

        int cipherTextLength = cipherBytes.Length - iv.Length;
        byte[] cipherText = new byte[cipherTextLength];
        Array.Copy(cipherBytes, iv.Length, cipherText, 0, cipherTextLength);

        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        using MemoryStream msDecrypt = new(cipherText);
        using CryptoStream csDecrypt = new(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}
