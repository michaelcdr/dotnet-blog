namespace Blog.Posts.API.Features.Categories.Shared;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QtdPosts { get; set; }
}
