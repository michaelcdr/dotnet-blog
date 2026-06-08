using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Auth.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Blog.Auth.Tests.Services;

public class AuthServiceTests
{
    private const string LoginErrorMessage = "N\u00e3o foi possivel logar.";
    private const string LoginSuccessMessage = "Logado com sucesso.";

    [Fact]
    public async Task Login_DeveRetornarSucesso_QuandoCredenciaisSaoValidasETokenForGerado()
    {
        var request = new UserLogin
        {
            UserName = "usuario@teste.com",
            Password = "123"
        };

        var tokenResponse = new TokenGeneratedResponse
        {
            AccessToken = "token-jwt",
            ExpiresIn = 7200,
            UserToken = new UserToken
            {
                Id = "user-id",
                UserName = request.UserName,
                Email = request.UserName
            }
        };

        var signInManager = CreateSignInManagerMock();
        signInManager
            .Setup(manager => manager.PasswordSignInAsync(request.UserName, request.Password, false, true))
            .ReturnsAsync(SignInResult.Success);

        var tokenGenerator = new Mock<ITokenGenerator>();
        tokenGenerator
            .Setup(generator => generator.Generate(request.UserName))
            .ReturnsAsync(tokenResponse);

        var service = CreateService(signInManager.Object, tokenGenerator.Object);

        var response = await service.Login(request);

        response.Success.Should().BeTrue();
        response.Message.Should().Be(LoginSuccessMessage);
        response.Errors.Should().BeEmpty();
        response.Data.Should().BeSameAs(tokenResponse);
        response.Data!.AccessToken.Should().Be("token-jwt");

        signInManager.Verify(
            manager => manager.PasswordSignInAsync(request.UserName, request.Password, false, true),
            Times.Once);
        tokenGenerator.Verify(generator => generator.Generate(request.UserName), Times.Once);
    }

    [Fact]
    public async Task Login_DeveRetornarErro_QuandoCredenciaisSaoInvalidas()
    {
        var request = new UserLogin
        {
            UserName = "usuario@teste.com",
            Password = "senha-invalida"
        };

        var signInManager = CreateSignInManagerMock();
        signInManager
            .Setup(manager => manager.PasswordSignInAsync(request.UserName, request.Password, false, true))
            .ReturnsAsync(SignInResult.Failed);

        var tokenGenerator = new Mock<ITokenGenerator>();
        var service = CreateService(signInManager.Object, tokenGenerator.Object);

        var response = await service.Login(request);

        response.Success.Should().BeFalse();
        response.Message.Should().Be(LoginErrorMessage);
        response.Data.Should().BeNull();
        response.Errors.Should().ContainSingle(error => error.Message == LoginErrorMessage && error.Property == "");

        tokenGenerator.Verify(generator => generator.Generate(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Login_DeveRetornarErro_QuandoTokenNaoForGerado()
    {
        var request = new UserLogin
        {
            UserName = "usuario@teste.com",
            Password = "123"
        };

        var signInManager = CreateSignInManagerMock();
        signInManager
            .Setup(manager => manager.PasswordSignInAsync(request.UserName, request.Password, false, true))
            .ReturnsAsync(SignInResult.Success);

        var tokenGenerator = new Mock<ITokenGenerator>();
        tokenGenerator
            .Setup(generator => generator.Generate(request.UserName))
            .ReturnsAsync((TokenGeneratedResponse?)null);

        var service = CreateService(signInManager.Object, tokenGenerator.Object);

        var response = await service.Login(request);

        response.Success.Should().BeFalse();
        response.Message.Should().Be(LoginErrorMessage);
        response.Data.Should().BeNull();
        response.Errors.Should().ContainSingle(error => error.Message == LoginErrorMessage && error.Property == "");

        tokenGenerator.Verify(generator => generator.Generate(request.UserName), Times.Once);
    }

    [Theory]
    [MemberData(nameof(UnsuccessfulSignInResults))]
    public async Task Login_DeveRetornarErro_QuandoIdentityNaoConcluirLogin(SignInResult signInResult)
    {
        var request = new UserLogin
        {
            UserName = "usuario@teste.com",
            Password = "123"
        };

        var signInManager = CreateSignInManagerMock();
        signInManager
            .Setup(manager => manager.PasswordSignInAsync(request.UserName, request.Password, false, true))
            .ReturnsAsync(signInResult);

        var tokenGenerator = new Mock<ITokenGenerator>();
        var service = CreateService(signInManager.Object, tokenGenerator.Object);

        var response = await service.Login(request);

        response.Success.Should().BeFalse();
        response.Message.Should().Be(LoginErrorMessage);
        response.Data.Should().BeNull();
        response.Errors.Should().ContainSingle(error => error.Message == LoginErrorMessage && error.Property == "");

        tokenGenerator.Verify(generator => generator.Generate(It.IsAny<string>()), Times.Never);
    }

    public static IEnumerable<object[]> UnsuccessfulSignInResults()
    {
        yield return [SignInResult.Failed];
        yield return [SignInResult.LockedOut];
        yield return [SignInResult.NotAllowed];
        yield return [SignInResult.TwoFactorRequired];
    }

    private static AuthService CreateService(
        SignInManager<IdentityUser> signInManager,
        ITokenGenerator tokenGenerator)
    {
        return new AuthService(CreateUserManagerMock().Object, signInManager, tokenGenerator);
    }

    private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        var options = Options.Create(new IdentityOptions());
        var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
        var userValidators = Array.Empty<IUserValidator<IdentityUser>>();
        var passwordValidators = Array.Empty<IPasswordValidator<IdentityUser>>();
        var normalizer = new Mock<ILookupNormalizer>();
        var errors = new IdentityErrorDescriber();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<IdentityUser>>>();

        return new Mock<UserManager<IdentityUser>>(
            store.Object,
            options,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            normalizer.Object,
            errors,
            services.Object,
            logger.Object);
    }

    private static Mock<SignInManager<IdentityUser>> CreateSignInManagerMock()
    {
        var userManager = CreateUserManagerMock();
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        var options = Options.Create(new IdentityOptions());
        var logger = new Mock<ILogger<SignInManager<IdentityUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        var confirmation = new Mock<IUserConfirmation<IdentityUser>>();

        return new Mock<SignInManager<IdentityUser>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            options,
            logger.Object,
            schemes.Object,
            confirmation.Object);
    }
}
