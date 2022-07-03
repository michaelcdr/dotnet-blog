namespace CodingBlog.Models.Data
{
    public interface IContexto
    {
        List<Categoria> Categorias { get; set; }
        List<Post> Posts { get; set; }
    }
}
