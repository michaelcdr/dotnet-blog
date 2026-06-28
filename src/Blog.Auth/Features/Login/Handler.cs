using Blog.Auth.Models;
using Blog.Auth.Services;
using Blog.Core.Models;

namespace Blog.Auth.Features.Login;

public class Handler
{
    private readonly IAuthService _authService;

    public Handler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AppResponse<Blog.Auth.Jwt.TokenGeneratedResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        return _authService.Login(new UserLogin
        {
            UserName = request.UserName,
            Password = request.Password
        }, cancellationToken);
    }
}
