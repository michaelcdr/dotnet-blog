using CodingBlog.Models;
using CodingBlog.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CodingBlog.Areas.Admin.Features.Login;

public class Handler
{
    private const string MissingAdminRoleMessage = "Sua conta nao possui acesso administrativo.";

    private readonly IAuthHttpService _authHttpService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<Handler> _logger;

    public Handler(
        IAuthHttpService authHttpService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<Handler> logger)
    {
        _authHttpService = authHttpService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var tokenResponse = await _authHttpService.Login(new LoginModel
        {
            UserName = request.UserName,
            Password = request.Password
        }, cancellationToken);

        var errors = tokenResponse.ResponseResult?.Errors?.Mensagens;
        if (errors?.Count > 0)
            return Response.Fail(errors[0]);

        if (!HasAdminRole(tokenResponse))
        {
            _logger.LogWarning("Usuario {UserName} autenticado sem a role admin.", request.UserName);
            await SignOut(cancellationToken);
            return Response.Fail(MissingAdminRoleMessage);
        }

        await SignIn(tokenResponse);
        return Response.Ok();
    }

    private static bool HasAdminRole(TokenResponse tokenResponse)
    {
        return tokenResponse.UserToken?.Claims?.Any(claim =>
            string.Equals(claim.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(claim.Value, "admin", StringComparison.OrdinalIgnoreCase)) == true;
    }

    private async Task SignIn(TokenResponse tokenResponse)
    {
        var claims = new List<Claim>
        {
            new("JWT", tokenResponse.AccessToken)
        };

        if (tokenResponse.UserToken?.Claims != null)
        {
            claims.AddRange(tokenResponse.UserToken.Claims
                .Where(claim => !string.IsNullOrWhiteSpace(claim.Type) && !string.IsNullOrWhiteSpace(claim.Value))
                .Select(claim => new Claim(claim.Type!, claim.Value!)));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {
            IssuedUtc = DateTime.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true
        };

        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("Contexto HTTP indisponivel durante a autenticacao.");

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);
    }

    private async Task SignOut(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_httpContextAccessor.HttpContext is null)
            return;

        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
