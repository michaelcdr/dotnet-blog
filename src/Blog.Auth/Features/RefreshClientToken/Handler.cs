using Blog.Auth.Models;
using Blog.Auth.Services;
using Blog.Core.Models;

namespace Blog.Auth.Features.RefreshClientToken;

public class Handler
{
    private readonly IClientAuthService _clientAuthService;

    public Handler(IClientAuthService clientAuthService)
    {
        _clientAuthService = clientAuthService;
    }

    public Task<AppResponse<ClientTokenResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        return _clientAuthService.RefreshToken(new ClientRefreshTokenRequest
        {
            ClientId = request.ClientId,
            RefreshToken = request.RefreshToken
        }, cancellationToken);
    }
}
