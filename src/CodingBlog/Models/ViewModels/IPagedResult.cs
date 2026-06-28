namespace CodingBlog.Models;

public interface IPagedResult
{
    int Page { get; }
    int TotalPages { get; }
}
