namespace Blog.Posts.API.Features.Categories.DeleteCategory;

public class Request
{
    public int Id { get; set; }
    public int? DestinationCategoryId { get; set; }
}
