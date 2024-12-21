using Blog.Core.Services;
using CodingBlog.Configuracoes;
using CodingBlog.Models;
using Microsoft.Extensions.Options;

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
        _httpClient = client;
    }

    public async Task CriarCategoria(CategoriaCadastroModel categoria)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoriaEdicaoModel> ObterCategoriaPorId(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CategoriaViewModel>> ObterCategorias()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/categorias");

        var carouselItems = await Deserialize<List<CategoriaViewModel>>(response);

        return carouselItems;
    }

    public async Task<List<string>> ObterTodasTags()
    {
        throw new NotImplementedException();
    }

    public async Task<List<PostRecenteViewModel>> ObterPostsRecentes()
    {
        throw new NotImplementedException();
    }

    public async Task<PostsPorCategoriaViewModel> ObterPostsPorCategoria(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/posts/por-categoria/{id}");

        var posts = await Deserialize<List<PostViewModel>>(response);

        HttpResponseMessage categoriaResposta = await _httpClient.GetAsync($"api/categoria/{id}");

        var categoria = await Deserialize<CategoriaViewModel>(response);

        return new PostsPorCategoriaViewModel(posts, categoria);
    }

    public async Task<List<PostViewModel>> ObterPostsPorTags(string tag)
    {
        throw new NotImplementedException();
    }

    public Task<PostDetalhesViewModel> ObterDetalhesPost(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostPesquisaViewModel>> ObterPostsPorTermoPesquisa(string pesquisa)
    {
        throw new NotImplementedException();
    }
}