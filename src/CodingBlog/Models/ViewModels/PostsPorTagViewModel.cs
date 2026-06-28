namespace CodingBlog.Models;

public class PostsPorTagViewModel
{
    public PostsPorTagViewModel(PagedResult<PostViewModel> posts)
    {
        Posts = posts.Items;
        Paginacao = posts;
    }

    public List<PostViewModel> Posts { get; }
    public PagedResult<PostViewModel> Paginacao { get; }
}
