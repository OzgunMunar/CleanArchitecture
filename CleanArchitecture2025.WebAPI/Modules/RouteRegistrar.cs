using CleanArchitecture2025.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.RegisterEmployeeRoutes();
        endpointRouteBuilder.RegisterAuthRoutes();
    }
}