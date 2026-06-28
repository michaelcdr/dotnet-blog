using Blog.Core.Models;

namespace Blog.Posts.API.Features.Posts.AdminListPosts;

public class Request : PagedRequest
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public string? SortBy { get; set; } = "id";
    public string? SortDirection { get; set; } = "desc";
}
