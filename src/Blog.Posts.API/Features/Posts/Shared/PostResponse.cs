namespace Blog.Posts.API.Features.Posts.Shared;

public class PostResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string? Imagem { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
}
