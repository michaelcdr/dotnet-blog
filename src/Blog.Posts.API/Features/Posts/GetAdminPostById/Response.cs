namespace Blog.Posts.API.Features.Posts.GetAdminPostById;

public class Response
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string? Imagem { get; set; }
    public int CategoriaId { get; set; }
}
