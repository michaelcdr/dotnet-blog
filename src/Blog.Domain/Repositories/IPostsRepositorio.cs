namespace Blog.Domain.Repositories;

public interface IPostsRepositorio
{
    List<Post> ObterRecentes();
    List<string> ObterTodasTags();
    List<Post> ObterPorTags(string tag);
    List<Post> ObterPorCategoria(int id);
    Task<Post> Obter(int id);
    List<Post> ObterPorTermoPesquisa(string pesquisa);
}