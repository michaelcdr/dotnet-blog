namespace Blog.Posts.API.Features.Posts.CreateAdminPost;

public class Request
{
    public int CategoriaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string? Imagem { get; set; }
}
