using CodingBlog.Models;

namespace CodingBlog.HttpClients;

public interface IBlogApiHttpClient
{
    Task CriarCategoria(CategoriaCadastroModel categoria);
    Task<CategoriaEdicaoModel> ObterCategoriaPorId(int id);
    Task<List<CategoriaViewModel>> ObterCategorias();
    Task<PostViewModel> ObterDetalhesPost(int id);
    Task<PostsPorCategoriaViewModel> ObterPostsPorCategoria(int id);
    Task<List<PostViewModel>> ObterPostsPorTags(string tag);
    Task<List<PostViewModel>> ObterPostsPorTermoPesquisa(string pesquisa = null);
    Task<List<PostRecenteViewModel>> ObterPostsRecentes();
    Task<List<string>> ObterTodasTags();
}