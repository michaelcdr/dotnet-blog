using CodingBlog.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Blog.Core.Services;
using CodingBlog.Configuracoes;

namespace CodingBlog.HttpClients;

public class AuthHttpService : ServiceBase, IAuthHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISerializerService _serializerService;

    public AuthHttpService(HttpClient client,
                           IHttpContextAccessor httpContextAccessor,
                           ISerializerService serializerService,
                           IOptions<AppSettings> options) : base(serializerService)
    {
        _httpClient = client;
    }

    public async Task<TokenResponse> Login(LoginModel loginModel)
    {
        HttpResponseMessage response = await _httpClient.PostAsync("api/conta/login", FormatContent(loginModel));

        if (!HandleResponseErrors(response))
            return new TokenResponse
            {
                ResponseResult = await Deserialize<ResponseResult>(response)
            };

        var result = await Deserialize<TokenResponse>(response);

        if (result == null) throw new InvalidOperationException("Resultado inválido ao tentar logar.");

        await AutenticarRegistrandoClaims(result);

        return result;
    }

    private async Task AutenticarRegistrandoClaims(TokenResponse tokenResult)
    {
        var claims = new List<Claim>
        {
            new Claim("JWT", tokenResult.AccessToken)
        };
        claims.AddRange(tokenResult.UserToken.Claims.Select(e => new Claim(e.Type, e.Value)).ToList());

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProp = new AuthenticationProperties
        {
            IssuedUtc = DateTime.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true
        };

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProp
        );
    }
}