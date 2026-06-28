using Blog.Core.Models;

namespace Blog.Posts.API.Features.Posts.SearchPosts;

public class Request : PagedRequest
{
    public string? Pesquisa { get; set; }
}
