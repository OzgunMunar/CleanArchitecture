using CleanArchitecture2025.Application.Auth;
using CleanArchitecture2025.Application.Employees;
using TS.MediatR;
using TS.Result;

namespace CleanArchitecture2025.WebAPI.Modules;
public static class AuthModule
{
    public static void RegisterAuthRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        RouteGroupBuilder group = endpointRouteBuilder.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login", async (ISender sender, LoginCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
        .Produces<Result<LoginCommand>>();

    }
}