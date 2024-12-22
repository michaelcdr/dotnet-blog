using Blog.Posts.Domain;

namespace Blog.Posts.Data.Context;

public interface IContexto
{
    List<Categoria> Categorias { get; set; }
    List<Post> Posts { get; set; }
}
