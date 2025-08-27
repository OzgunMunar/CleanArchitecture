using CleanArchitecture2025.Domain.Users;

namespace CleanArchitecture2025.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
}