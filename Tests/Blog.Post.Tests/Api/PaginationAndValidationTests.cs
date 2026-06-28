using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blog.Post.Tests.Api;

public class PaginationAndValidationTests : IClassFixture<PostApiFactory>
{
    private readonly PostApiFactory _factory;

    public PaginationAndValidationTests(PostApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Posts_DeveRetornarEnvelopePaginado()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.GetAsync("/api/posts?page=1&pageSize=1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        root.GetProperty("page").GetInt32().Should().Be(1);
        root.GetProperty("pageSize").GetInt32().Should().Be(1);
        root.GetProperty("totalItems").GetInt32().Should().BeGreaterThanOrEqualTo(1);
        root.GetProperty("totalPages").GetInt32().Should().BeGreaterThanOrEqualTo(1);
        root.GetProperty("items").GetArrayLength().Should().BeLessThanOrEqualTo(1);
    }

    [Fact]
    public async Task Categorias_DeveLimitarPageSizeAoMaximo()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.GetAsync("/api/categorias?page=1&pageSize=999");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        document.RootElement.GetProperty("pageSize").GetInt32().Should().Be(50);
    }

    [Fact]
    public async Task CriarCategoria_DeveRetornarBadRequest_QuandoNomeNaoForInformado()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.PostAsJsonAsync("/api/categorias", new { Nome = "" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Informe o nome da categoria.");
    }

    [Fact]
    public async Task CategoriasAdmin_DevePermitirBuscaEPaginacao()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        await client.PostAsJsonAsync("/api/categorias", new { Nome = "Arquitetura" });
        await client.PostAsJsonAsync("/api/categorias", new { Nome = "Arquivos" });

        var response = await client.GetAsync("/api/categorias/admin?page=1&pageSize=1&search=Arq&sortBy=nome&sortDirection=asc");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        root.GetProperty("page").GetInt32().Should().Be(1);
        root.GetProperty("pageSize").GetInt32().Should().Be(1);
        root.GetProperty("totalItems").GetInt32().Should().BeGreaterThanOrEqualTo(2);
        root.GetProperty("items").GetArrayLength().Should().Be(1);
    }

    [Fact]
    public async Task RemoverCategoria_ComPosts_DeveExigirDestino()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.DeleteAsync("/api/categorias/1");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Escolha uma categoria de destino");
    }

    [Fact]
    public async Task PostsAdmin_DevePermitirBusca()
    {
        var client = _factory.CreateAuthenticatedClient(JwtTokenBuilder.CreateValidToken());

        var response = await client.GetAsync("/api/posts/admin?page=1&pageSize=10&search=Dapper");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = document.RootElement.GetProperty("items");

        items.GetArrayLength().Should().BeGreaterThanOrEqualTo(1);
        items[0].GetProperty("titulo").GetString().Should().NotBeNullOrWhiteSpace();
    }
}
