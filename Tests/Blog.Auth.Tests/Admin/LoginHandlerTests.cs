using CodingBlog.Areas.Admin.Features.Login;
using CodingBlog.Models;
using CodingBlog.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Blog.Auth.Tests.Admin;

public class LoginHandlerTests
{
    [Fact]
    public async Task Handle_DeveEnviarCredenciaisEPropagarCancellationToken_QuandoLoginForValido()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var capturedToken = CancellationToken.None;
        LoginModel? capturedLogin = null;

        var authHttpService = new Mock<IAuthHttpService>();
        authHttpService
            .Setup(service => service.Login(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
            .Callback<LoginModel, CancellationToken>((login, token) =>
            {
                capturedLogin = login;
                capturedToken = token;
            })
            .ReturnsAsync(BuildTokenResponse(ClaimTypes.Role, "admin"));

        var handler = CreateHandler(authHttpService.Object);

        var result = await handler.Handle(new Request
        {
            UserName = "michael",
            Password = "giacom"
        }, cancellationToken);

        result.Success.Should().BeTrue();
        capturedLogin.Should().NotBeNull();
        capturedLogin!.UserName.Should().Be("michael");
        capturedLogin.Password.Should().Be("giacom");
        capturedToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task Handle_DeveRetornarErroAmigavel_QuandoCredenciaisForemInvalidas()
    {
        var authHttpService = new Mock<IAuthHttpService>();
        authHttpService
            .Setup(service => service.Login(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildErrorResponse("Usuario ou senha invalidos."));

        var handler = CreateHandler(authHttpService.Object);

        var result = await handler.Handle(new Request
        {
            UserName = "michael",
            Password = "senha-errada"
        }, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Usuario ou senha invalidos.");
    }

    [Fact]
    public async Task Handle_DeveRetornarErroAmigavel_QuandoApiAuthEstiverIndisponivel()
    {
        var authHttpService = new Mock<IAuthHttpService>();
        authHttpService
            .Setup(service => service.Login(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildErrorResponse("Nao foi possivel autenticar agora. Tente novamente em instantes."));

        var handler = CreateHandler(authHttpService.Object);

        var result = await handler.Handle(new Request
        {
            UserName = "michael",
            Password = "giacom"
        }, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Nao foi possivel autenticar agora. Tente novamente em instantes.");
    }

    [Fact]
    public async Task Handle_DeveRecusarUsuarioSemRoleAdmin()
    {
        var authHttpService = new Mock<IAuthHttpService>();
        authHttpService
            .Setup(service => service.Login(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildTokenResponse(ClaimTypes.Role, "comum"));

        var handler = CreateHandler(authHttpService.Object);

        var result = await handler.Handle(new Request
        {
            UserName = "michael",
            Password = "giacom"
        }, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Sua conta nao possui acesso administrativo.");
    }

    private static Handler CreateHandler(IAuthHttpService authHttpService)
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .Services
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services
        };

        return new Handler(
            authHttpService,
            new HttpContextAccessor { HttpContext = httpContext },
            Mock.Of<ILogger<Handler>>());
    }

    private static TokenResponse BuildTokenResponse(string claimType, string claimValue)
    {
        return new TokenResponse
        {
            AccessToken = "jwt-token",
            UserToken = new CodingBlog.Models.UserToken
            {
                Claims =
                [
                    new CodingBlog.Models.UserClaim
                    {
                        Type = claimType,
                        Value = claimValue
                    }
                ]
            }
        };
    }

    private static TokenResponse BuildErrorResponse(string message)
    {
        return new TokenResponse
        {
            ResponseResult = new ResponseResult
            {
                Errors = new ResponseErrorMessages
                {
                    Mensagens = [message]
                }
            }
        };
    }
}
