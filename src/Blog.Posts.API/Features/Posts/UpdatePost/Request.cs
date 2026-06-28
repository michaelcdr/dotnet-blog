namespace Blog.Posts.API.Features.Posts.UpdatePost;

public class Request
{
    public int Id { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;
}
