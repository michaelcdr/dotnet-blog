using FluentAssertions;
using System.Net;

namespace Blog.Post.Tests.Api;

public class EndpointAuthorizationTests : IClassFixture<PostApiFactory>
{
    private readonly PostApiFactory _factory;

    public EndpointAuthorizationTests(PostApiFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/posts")]
    [InlineData("/api/categorias")]
    public async Task Endpoints_DeveRetornarUnauthorized_QuandoTokenNaoForInformado(string endpoint)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/api/posts")]
    [InlineData("/api/categorias")]
    public async Task Endpoints_DeveRetornarUnauthorized_QuandoTokenForInvalido(string endpoint)
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateTokenWithInvalidSignature());

        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/api/posts")]
    [InlineData("/api/categorias")]
    public async Task Endpoints_DeveRetornarUnauthorized_QuandoTokenEstiverExpirado(string endpoint)
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateExpiredToken());

        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/api/posts")]
    [InlineData("/api/categorias")]
    public async Task Endpoints_DevePermitirAcesso_QuandoTokenForValido(string endpoint)
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
