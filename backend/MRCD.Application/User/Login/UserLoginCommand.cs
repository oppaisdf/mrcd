using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.User.DTOs;

namespace MRCD.Application.User.Login;

public sealed record UserLoginCommand(
    string Username,
    string Password
) : ICommand<LoginDTO>;