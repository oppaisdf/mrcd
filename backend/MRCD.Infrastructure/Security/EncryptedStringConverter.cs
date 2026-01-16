using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MRCD.Application.Abstracts.Security;

namespace MRCD.Infrastructure.Security;

internal sealed class EncryptedStringConverter(
    IEncryptionService service
) : ValueConverter<string?, string?>(
    v => service.Encrypt(v),
    v => service.Decrypt(v)
)
{ }

internal sealed class EncryptedRequiredStringConverter(
    IEncryptionService service
) : ValueConverter<string, string>(
    v => service.Encrypt(v)!,
    v => service.Decrypt(v)!
)
{ }