using Blog.Auth.Models;
using Blog.Auth.Services;
using Blog.Core.Models;

namespace Blog.Auth.Features.GenerateClientToken;

public class Handler
{
    private readonly IClientAuthService _clientAuthService;

    public Handler(IClientAuthService clientAuthService)
    {
        _clientAuthService = clientAuthService;
    }

    public Task<AppResponse<ClientTokenResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        return _clientAuthService.GenerateToken(new ClientTokenRequest
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret
        }, cancellationToken);
    }
}
