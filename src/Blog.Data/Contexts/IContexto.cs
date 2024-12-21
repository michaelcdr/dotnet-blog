using Blog.Domain;

namespace Blog.Data.Context;

public interface IContexto
{
    List<Categoria> Categorias { get; set; }
    List<Post> Posts { get; set; }
}
