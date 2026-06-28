using CodingBlog.Models;
using CodingBlog.Models.Admin;

namespace CodingBlog.Services;

public interface IBlogApiService
{
    Task CriarCategoria(CategoriaCadastroModel categoria);
    Task<CategoriaEdicaoModel> ObterCategoriaPorId(int id);
    Task<PagedResult<CategoriaViewModel>> ObterCategorias(int page = 1, int pageSize = 10);
    Task<PostViewModel> ObterDetalhesPost(int id);
    Task<PostsPorCategoriaViewModel> ObterPostsPorCategoria(int id, int page = 1, int pageSize = 10);
    Task<PagedResult<PostViewModel>> ObterPostsPorTags(string tag, int page = 1, int pageSize = 10);
    Task<PagedResult<PostViewModel>> ObterPostsPorTermoPesquisa(string? pesquisa = null, int page = 1, int pageSize = 10);
    Task<List<PostRecenteViewModel>> ObterPostsRecentes();
    Task<List<string>> ObterTodasTags();
    Task<PagedResult<CategoriaViewModel>> ObterCategoriasAdmin(AdminGridRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CategoryOptionViewModel>> ObterOpcoesCategorias(CancellationToken cancellationToken = default);
    Task CriarCategoriaAdmin(AdminCategoryFormViewModel categoria, CancellationToken cancellationToken = default);
    Task AtualizarCategoriaAdmin(AdminCategoryFormViewModel categoria, CancellationToken cancellationToken = default);
    Task RemoverCategoriaAdmin(int id, int? destinationCategoryId, CancellationToken cancellationToken = default);
    Task<PagedResult<PostViewModel>> ObterPostsAdmin(AdminGridRequest request, CancellationToken cancellationToken = default);
    Task<AdminPostFormViewModel?> ObterPostAdminPorId(int id, CancellationToken cancellationToken = default);
    Task<int> CriarPostAdmin(AdminPostFormViewModel post, CancellationToken cancellationToken = default);
    Task AtualizarPostAdmin(AdminPostFormViewModel post, CancellationToken cancellationToken = default);
    Task RemoverPostAdmin(int id, CancellationToken cancellationToken = default);
}