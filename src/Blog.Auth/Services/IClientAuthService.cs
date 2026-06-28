using Blog.Auth.Models;
using Blog.Core.Models;

namespace Blog.Auth.Services;

public interface IClientAuthService
{
    Task<AppResponse<ClientTokenResponse>> GenerateToken(ClientTokenRequest request, CancellationToken cancellationToken = default);
    Task<AppResponse<ClientTokenResponse>> RefreshToken(ClientRefreshTokenRequest request, CancellationToken cancellationToken = default);
}
