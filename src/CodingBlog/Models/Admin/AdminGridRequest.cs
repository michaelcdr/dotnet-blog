namespace CodingBlog.Models.Admin;

public class AdminGridRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int? CategoryId { get; set; }
}
