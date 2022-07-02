
using CodingBlog.Models;

namespace CodingBlog.Interfaces
{
    public interface IPostsRepositorio
    {
        List<Post> ObterRecentes();
        List<string> ObterTodasTags();
        List<Post> ObterPorTags(string tag);
        List<Post> ObterPorCategoria(int id);
        Post Obter(int id);
    }
}