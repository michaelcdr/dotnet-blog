using Blog.Auth.Jwt;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.Auth.Tests.Services;

public class TokenGeneratorTests
{
    private const string ValidSecret = "uma-chave-secreta-com-mais-de-32-bytes";

    [Fact]
    public async Task Generate_DeveCriarTokenComClaimsRolesEExpiracaoConfigurada()
    {
        var identityUser = new IdentityUser("usuario@teste.com")
        {
            Id = "user-id",
            Email = "usuario@teste.com"
        };

        var userManager = CreateUserManagerMock();
        userManager
            .Setup(manager => manager.FindByNameAsync(identityUser.UserName!))
            .ReturnsAsync(identityUser);
        userManager
            .Setup(manager => manager.GetRolesAsync(identityUser))
            .ReturnsAsync(["admin"]);
        userManager
            .Setup(manager => manager.GetClaimsAsync(identityUser))
            .ReturnsAsync(new List<Claim>());

        var jwtSettings = new JwtAppSettings
        {
            Secret = ValidSecret,
            ExpiresIn = 1,
            Issuer = "blog-auth",
            Audience = "blog-api"
        };

        var service = CreateService(userManager.Object, jwtSettings);

        var before = DateTime.UtcNow;
        var response = await service.Generate(identityUser.UserName!);
        var after = DateTime.UtcNow;

        response.Should().NotBeNull();
        response!.ExpiresIn.Should().Be(TimeSpan.FromHours(jwtSettings.ExpiresIn).TotalSeconds);
        response.UserToken!.Claims.Should().Contain(claim => claim.Type == ClaimTypes.Role && claim.Value == "admin");

        var token = new JwtSecurityTokenHandler().ReadJwtToken(response.AccessToken);
        token.Issuer.Should().Be(jwtSettings.Issuer);
        token.Audiences.Should().Contain(jwtSettings.Audience);
        token.Claims.Should().Contain(claim => claim.Type == "role" && claim.Value == "admin");
        token.ValidTo.Should().BeOnOrAfter(before.AddHours(jwtSettings.ExpiresIn).AddSeconds(-5));
        token.ValidTo.Should().BeOnOrBefore(after.AddHours(jwtSettings.ExpiresIn).AddSeconds(5));
    }

    [Fact]
    public async Task Generate_DeveRetornarNull_QuandoUsuarioNaoTemRoles()
    {
        var identityUser = new IdentityUser("usuario@teste.com")
        {
            Id = "user-id",
            Email = "usuario@teste.com"
        };

        var userManager = CreateUserManagerMock();
        userManager
            .Setup(manager => manager.FindByNameAsync(identityUser.UserName!))
            .ReturnsAsync(identityUser);
        userManager
            .Setup(manager => manager.GetRolesAsync(identityUser))
            .ReturnsAsync([]);

        var service = CreateService(userManager.Object, new JwtAppSettings
        {
            Secret = ValidSecret,
            ExpiresIn = 1,
            Issuer = "blog-auth",
            Audience = "blog-api"
        });

        var response = await service.Generate(identityUser.UserName!);

        response.Should().BeNull();
        userManager.Verify(manager => manager.GetClaimsAsync(It.IsAny<IdentityUser>()), Times.Never);
    }

    private static TokenGenerator CreateService(
        UserManager<IdentityUser> userManager,
        JwtAppSettings jwtSettings)
    {
        return new TokenGenerator(
            userManager,
            Options.Create(jwtSettings),
            Mock.Of<ILogger<TokenGenerator>>());
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
}
