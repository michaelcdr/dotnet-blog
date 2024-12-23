namespace Blog.Posts.Domain.Services;

public interface IPostService
{
    Task Publish(string titulo, string categoria, string description, List<string> tags, string imagem);
}
