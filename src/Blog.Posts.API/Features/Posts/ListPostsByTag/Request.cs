using Blog.Core.Models;

namespace Blog.Posts.API.Features.Posts.ListPostsByTag;

public class Request : PagedRequest
{
    public string? Tag { get; set; }
}
