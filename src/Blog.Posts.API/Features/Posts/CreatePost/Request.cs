namespace Blog.Posts.API.Features.Posts.CreatePost;

public class Request
{
    public string Categoria { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;
}
