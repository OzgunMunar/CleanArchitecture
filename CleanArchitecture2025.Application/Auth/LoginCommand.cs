using CleanArchitecture2025.Application.Services;
using CleanArchitecture2025.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;
using TS.Result;

namespace CleanArchitecture2025.Application.Auth;
public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password
) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;

    // letting people to leave their process for anything
    // so when they get back after some time, they can continue

    // public string RefreshToken { get; set; } = default!;
}

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvider
) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync
            (user =>
                user.Email == request.UserNameOrEmail
                ||
                user.UserName == request.UserNameOrEmail, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("User could not found");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
            {
                return (500, $"You entered your password 5 times in a row. Therefore, your account is locked for {Math.Ceiling(timeSpan.Value.TotalMinutes)} minutes.");
            }
            else
            {
                return (500, $"You entered your password 5 times in a row. Therefore, your account is locked for 5 minutes.");
            }
        }

        if (signInResult.IsNotAllowed)
        {
            return (500, "Please verify your email address.");
        }

        if (!signInResult.Succeeded)
        {
            return (500, "Your password is wrong.");
        }

        var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);

        var response = new LoginCommandResponse()
        {
            AccessToken = token
        };

        return response;

    }
}