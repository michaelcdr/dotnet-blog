using Blog.Posts.Domain;

namespace Blog.Posts.API.DTO;

public class PostDTO
{
    public int Id { get; set; }
    public string Categoria { get; set; } =string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;

    public PostDTO()
    {
        
    }

    public PostDTO(Post post)
    {
        Id = post.Id;
        Categoria = post.Categoria?.Nome ?? string.Empty;
        Titulo = post.Titulo;
        Descritivo = post.Descritivo;
        Tags = post.Tags;
        Imagem = post.Imagem;
    }
}