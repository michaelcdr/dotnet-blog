namespace CodingBlog.Models;

public class PostsPorCategoriaViewModel
{
    public PostsPorCategoriaViewModel(PagedResult<PostViewModel> posts, CategoriaViewModel categoria)
    {
        Posts = posts.Items;
        Paginacao = posts;
        Categoria = categoria;
    }

    public List<PostViewModel> Posts { get; }
    public PagedResult<PostViewModel> Paginacao { get; }
    public CategoriaViewModel Categoria { get; set; }
}
