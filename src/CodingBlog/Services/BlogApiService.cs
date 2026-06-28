using Blog.Core.Services;
using CodingBlog.Configuracoes;
using CodingBlog.Models;
using CodingBlog.Models.Admin;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace CodingBlog.Services;

public class BlogApiService : ServiceBase, IBlogApiService
{
    private readonly HttpClient _httpClient;

    public BlogApiService(HttpClient client,
                          IHttpContextAccessor httpContextAccessor,
                          ISerializerService serializerService,
                          IOptions<AppSettings> options) : base(serializerService)
    {
        client.BaseAddress = new Uri(options.Value.UrlPostsApi);
        _httpClient = client;
    }

    public async Task CriarCategoria(CategoriaCadastroModel categoria)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/categorias", categoria);
        response.EnsureSuccessStatusCode();
    }

    public async Task<CategoriaEdicaoModel> ObterCategoriaPorId(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/categorias/{id}");
        return await Deserializar<CategoriaEdicaoModel>(response);
    }

    public async Task<PagedResult<CategoriaViewModel>> ObterCategorias(int page = 1, int pageSize = 10)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/categorias?page={page}&pageSize={pageSize}");
        return await Deserializar<PagedResult<CategoriaViewModel>>(response);
    }

    public async Task<List<string>> ObterTodasTags()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/posts/tags");
        return await Deserializar<List<string>>(response);
    }

    public async Task<PostViewModel> ObterDetalhesPost(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/{id}");
        return await Deserializar<PostViewModel>(response);
    }

    public async Task<List<PostRecenteViewModel>> ObterPostsRecentes()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/posts/recentes");
        return await Deserializar<List<PostRecenteViewModel>>(response);
    }

    public async Task<PostsPorCategoriaViewModel> ObterPostsPorCategoria(int id, int page = 1, int pageSize = 10)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/por-categoria/{id}?page={page}&pageSize={pageSize}");
        var posts = await Deserializar<PagedResult<PostViewModel>>(response);

        HttpResponseMessage categoriaResposta = await _httpClient.GetAsync($"api/categorias/{id}");
        var categoria = await Deserializar<CategoriaViewModel>(categoriaResposta);

        return new PostsPorCategoriaViewModel(posts, categoria);
    }

    public async Task<PagedResult<PostViewModel>> ObterPostsPorTags(string tag, int page = 1, int pageSize = 10)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/por-tag/{tag}?page={page}&pageSize={pageSize}");
        return await Deserializar<PagedResult<PostViewModel>>(response);
    }

    public async Task<PagedResult<PostViewModel>> ObterPostsPorTermoPesquisa(string? pesquisa = null, int page = 1, int pageSize = 10)
    {
        var route = string.IsNullOrWhiteSpace(pesquisa) ? "api/posts/pesquisa" : $"api/posts/pesquisa/{pesquisa}";
        HttpResponseMessage response = await _httpClient.GetAsync($"{route}?page={page}&pageSize={pageSize}");
        return await Deserializar<PagedResult<PostViewModel>>(response);
    }

    public async Task<PagedResult<CategoriaViewModel>> ObterCategoriasAdmin(AdminGridRequest request, CancellationToken cancellationToken = default)
    {
        var query = BuildAdminQuery(request);
        var response = await _httpClient.GetAsync($"api/categorias/admin{query}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await Deserializar<PagedResult<CategoriaViewModel>>(response);
    }

    public async Task<IReadOnlyCollection<CategoryOptionViewModel>> ObterOpcoesCategorias(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/categorias/admin/options", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await Deserializar<List<CategoryOptionViewModel>>(response);
    }

    public async Task CriarCategoriaAdmin(AdminCategoryFormViewModel categoria, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/categorias", new { categoria.Nome }, cancellationToken);
        await EnsureSuccess(response);
    }

    public async Task AtualizarCategoriaAdmin(AdminCategoryFormViewModel categoria, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/categorias/{categoria.Id}", new { categoria.Nome }, cancellationToken);
        await EnsureSuccess(response);
    }

    public async Task RemoverCategoriaAdmin(int id, int? destinationCategoryId, CancellationToken cancellationToken = default)
    {
        var url = destinationCategoryId.HasValue
            ? $"api/categorias/{id}?destinationCategoryId={destinationCategoryId.Value}"
            : $"api/categorias/{id}";

        var response = await _httpClient.DeleteAsync(url, cancellationToken);
        await EnsureSuccess(response);
    }

    public async Task<PagedResult<PostViewModel>> ObterPostsAdmin(AdminGridRequest request, CancellationToken cancellationToken = default)
    {
        var query = BuildAdminQuery(request);

        if (request.CategoryId.HasValue)
            query = query.Contains('?')
                ? $"{query}&categoryId={request.CategoryId.Value}"
                : $"?categoryId={request.CategoryId.Value}";

        var response = await _httpClient.GetAsync($"api/posts/admin{query}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await Deserializar<PagedResult<PostViewModel>>(response);
    }

    public async Task<AdminPostFormViewModel?> ObterPostAdminPorId(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/posts/admin/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        var post = await Deserializar<AdminPostFormViewModel>(response);
        post.Categories = await ObterOpcoesCategorias(cancellationToken);
        return post;
    }

    public async Task<int> CriarPostAdmin(AdminPostFormViewModel post, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/posts/admin", new
        {
            post.CategoriaId,
            post.Titulo,
            post.Descritivo,
            post.Tags,
            post.Imagem
        }, cancellationToken);

        await EnsureSuccess(response);
        var payload = await Deserializar<CreatedIdResponse>(response);
        return payload.Id;
    }

    public async Task AtualizarPostAdmin(AdminPostFormViewModel post, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/posts/admin/{post.Id}", new
        {
            post.CategoriaId,
            post.Titulo,
            post.Descritivo,
            post.Tags,
            post.Imagem
        }, cancellationToken);

        await EnsureSuccess(response);
    }

    public async Task RemoverPostAdmin(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/posts/admin/{id}", cancellationToken);
        await EnsureSuccess(response);
    }

    private static string BuildAdminQuery(AdminGridRequest request)
    {
        var query = new List<string>
        {
            $"page={request.Page}",
            $"pageSize={request.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(request.Search))
            query.Add($"search={Uri.EscapeDataString(request.Search)}");

        if (!string.IsNullOrWhiteSpace(request.SortBy))
            query.Add($"sortBy={Uri.EscapeDataString(request.SortBy)}");

        if (!string.IsNullOrWhiteSpace(request.SortDirection))
            query.Add($"sortDirection={Uri.EscapeDataString(request.SortDirection)}");

        return $"?{string.Join("&", query)}";
    }

    private async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var payload = await Deserializar<ApiValidationProblem>(response);
            var errors = payload.Errors.SelectMany(item => item.Value).ToArray();
            throw new ApiValidationException(errors);
        }

        response.EnsureSuccessStatusCode();
    }

    private sealed class CreatedIdResponse
    {
        public int Id { get; set; }
    }
}