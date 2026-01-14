using MRCD.API.DTOs;

namespace MRCD.API.Services;

public interface ITokenService
{
    TokenDTO Create(Guid subject, IEnumerable<string> roles, IEnumerable<string> permissions);
}