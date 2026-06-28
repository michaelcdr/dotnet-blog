using Blog.Core.Models;

namespace Blog.Posts.API.Features.Categories.AdminListCategories;

public class Request : PagedRequest
{
    public string? Search { get; set; }
    public string? SortBy { get; set; } = "nome";
    public string? SortDirection { get; set; } = "asc";
}
