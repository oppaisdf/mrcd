namespace MRCD.Application.Abstracts.Security;

public interface IEncryptionService
{
    string? Encrypt(string? plaintext);
    string? Decrypt(string? payload);
}