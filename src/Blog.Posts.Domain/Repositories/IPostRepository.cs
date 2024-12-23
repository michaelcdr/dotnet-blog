using Blog.Core.Data;

namespace Blog.Posts.Domain.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<List<Post>> ObterRecentes();
    Task<List<string>> ObterTodasTags();
    Task<List<Post>> ObterPorTags(string tag);
    Task<List<Post>> ObterPorCategoria(int id);
    Task<Post> Obter(int id);
    Task<List<Post>> ObterPorTermoPesquisa(string pesquisa);
    void Add(Post postCriado);
}