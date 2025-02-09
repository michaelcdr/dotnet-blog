using Blog.Core.Services;
using CodingBlog.Configuracoes;
using CodingBlog.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CodingBlog.HttpClients;

public class BlogApiHttpClient : ServiceBase, IBlogApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISerializerService _serializerService;

    public BlogApiHttpClient(HttpClient client,
                             IHttpContextAccessor httpContextAccessor,
                             ISerializerService serializerService,
                             IOptions<AppSettings> options) : base(serializerService)
    {
        client.BaseAddress = new Uri(options.Value.UrlPostsApi);

        _httpClient = client;
    }

    public async Task CriarCategoria(CategoriaCadastroModel categoria)
    {
        var stringContent = new StringContent(
            JsonSerializer.Serialize(categoria)
        );

        HttpResponseMessage response = await _httpClient.PostAsync($"api/categorias", stringContent);

        response.EnsureSuccessStatusCode(); 
    }

    public async Task<CategoriaEdicaoModel> ObterCategoriaPorId(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/categorias/{id}");

        var categoria = await Deserialize<CategoriaEdicaoModel>(response);

        return categoria;
    }

    public async Task<List<CategoriaViewModel>> ObterCategorias()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/categorias");

        var dados = await Deserialize<List<CategoriaViewModel>>(response);

        return dados;
    }

    public async Task<List<string>> ObterTodasTags()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/posts/tags");

        var dados = await Deserialize<List<string>>(response);

        return dados;
    }

    public async Task<List<PostRecenteViewModel>> ObterPostsRecentes()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/recentes");

        var posts = await Deserialize<List<PostRecenteViewModel>>(response);

        return posts;
    }

    public async Task<PostsPorCategoriaViewModel> ObterPostsPorCategoria(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/por-categoria/{id}");

        var posts = await Deserialize<List<PostViewModel>>(response);

        HttpResponseMessage categoriaResposta = await _httpClient.GetAsync($"api/categorias/{id}");

        var categoria = await Deserialize<CategoriaViewModel>(categoriaResposta);

        return new PostsPorCategoriaViewModel(posts, categoria);
    }

    public async Task<List<PostViewModel>> ObterPostsPorTags(string tag)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/por-tag/{tag}");

        var posts = await Deserialize<List<PostViewModel>>(response);

        return posts;
    }

    public async Task<PostViewModel> ObterDetalhesPost(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/{id}");

        var post = await Deserialize<PostViewModel>(response);

        return post;
    }

    public async Task<List<PostViewModel>> ObterPostsPorTermoPesquisa(string pesquisa)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/pesquisa/{pesquisa}");

        var posts = await Deserialize<List<PostViewModel>>(response);

        return posts;
    }
}