using Blog.Auth.Configurations;
using Blog.Auth.Data;
using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Auth.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog.Auth.Tests.Services;

public class ClientAuthServiceTests
{
    private const string ClientId = "codingblog-site";
    private const string ClientSecret = "codingblog-development-client-secret-32-bytes";

    [Fact]
    public async Task GenerateToken_DeveRetornarToken_QuandoCredenciaisForemValidas()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var response = await service.GenerateToken(new ClientTokenRequest
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret
        });

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        response.Data.ExpiresIn.Should().Be(7200);
        context.ClientRefreshTokens.Should().ContainSingle(token => token.ClientId == ClientId);
    }

    [Fact]
    public async Task GenerateToken_DeveRetornarErro_QuandoCredenciaisForemInvalidas()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var response = await service.GenerateToken(new ClientTokenRequest
        {
            ClientId = ClientId,
            ClientSecret = "secret-invalido-com-mais-de-32-bytes"
        });

        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
        context.ClientRefreshTokens.Should().BeEmpty();
    }

    [Fact]
    public async Task RefreshToken_DeveRotacionarRefreshToken_QuandoTokenForValido()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var tokenResponse = await service.GenerateToken(new ClientTokenRequest
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret
        });

        var refreshResponse = await service.RefreshToken(new ClientRefreshTokenRequest
        {
            ClientId = ClientId,
            RefreshToken = tokenResponse.Data!.RefreshToken
        });

        refreshResponse.Success.Should().BeTrue();
        refreshResponse.Data!.RefreshToken.Should().NotBe(tokenResponse.Data.RefreshToken);
        context.ClientRefreshTokens.Count().Should().Be(2);
        context.ClientRefreshTokens.Count(token => token.RevokedAtUtc != null).Should().Be(1);

        var reusedTokenResponse = await service.RefreshToken(new ClientRefreshTokenRequest
        {
            ClientId = ClientId,
            RefreshToken = tokenResponse.Data.RefreshToken
        });

        reusedTokenResponse.Success.Should().BeFalse();
    }

    private static ClientAuthService CreateService(AuthContext context)
    {
        var jwtSettings = Options.Create(new JwtAppSettings
        {
            Secret = "development-secret-key-with-more-than-32-bytes",
            ExpiresIn = 2,
            Issuer = "blog-auth",
            Audience = "blog-posts-api"
        });

        var clients = Options.Create(new AuthClientsOptions
        {
            Clients =
            [
                new AuthClientOptions
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                }
            ]
        });

        return new ClientAuthService(
            context,
            new ClientTokenGenerator(jwtSettings),
            clients);
    }

    private static AuthContext CreateContext()
    {
        var databaseName = $"CodingBlog_Auth_Tests_{Guid.NewGuid():N}";
        var connectionString = $"Server=(localdb)\\SGPLocalDB;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False";

        var options = new DbContextOptionsBuilder<AuthContext>()
            .UseSqlServer(connectionString)
            .Options;

        var context = new TestAuthContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    private sealed class TestAuthContext : AuthContext
    {
        public TestAuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await Database.EnsureDeletedAsync();
            await base.DisposeAsync();
        }
    }
}
