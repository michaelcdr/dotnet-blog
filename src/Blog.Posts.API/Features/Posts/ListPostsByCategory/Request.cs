using Blog.Core.Models;

namespace Blog.Posts.API.Features.Posts.ListPostsByCategory;

public class Request : PagedRequest
{
    public int CategoriaId { get; set; }
}
