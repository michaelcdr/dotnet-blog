namespace Blog.Posts.API.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public string Categoria { get; set; } =string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public string Imagem { get; set; } = string.Empty;
}